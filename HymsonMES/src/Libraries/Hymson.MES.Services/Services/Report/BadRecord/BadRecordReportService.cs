using AutoMapper.Execution;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.NioPushCollection.Query;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.NioPushCollection;
using Hymson.MES.Services.Dtos.Report;
using Hymson.Minio;
using Hymson.Utils;
using Minio.DataModel;

namespace Hymson.MES.Services.Services.Report
{
    /// <summary>
    /// 不良记录报表 服务
    /// </summary>
    public class BadRecordReportService : IBadRecordReportService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 工单信息表 仓储
        /// </summary>
        private readonly IManuProductBadRecordRepository _manuProductBadRecordRepository;

        /// <summary>
        /// 仓储接口（产品NG记录表）
        /// </summary>
        private readonly IManuProductNgRecordRepository _manuProductNgRecordRepository;

        /// <summary>
        /// 仓储接口（不合格代码）
        /// </summary>
        private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;

        private readonly IExcelService _excelService;
        private readonly ILocalizationService _localizationService;
        private readonly IMinioService _minioService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// /// <param name="manuProductBadRecordRepository"></param>
        /// <param name="manuProductNgRecordRepository"></param>
        /// /// <param name="qualUnqualifiedCodeRepository"></param>
        public BadRecordReportService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuProductBadRecordRepository manuProductBadRecordRepository,
            IManuProductNgRecordRepository manuProductNgRecordRepository,
            IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository,
            IExcelService excelService,
            ILocalizationService localizationService,
            IMinioService minioService
            )
        {
            _currentUser = currentUser;
            _currentSite = currentSite;

            _manuProductBadRecordRepository = manuProductBadRecordRepository;
            _manuProductNgRecordRepository = manuProductNgRecordRepository;
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
            _excelService = excelService;
            _localizationService = localizationService;
            _minioService = minioService;
        }

        /// <summary>
        /// 根据查询条件获取不良报表分页数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuProductBadRecordReportViewDto>> GetPageListAsync(BadRecordReportDto param)
        {
            var pagedQuery = param.ToQuery<ManuProductBadRecordReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _manuProductBadRecordRepository.GetPagedInfoReportAsync(pagedQuery);

            var unqualifiedIds = pagedInfo.Data.Select(x => x.UnqualifiedId).ToArray();
            var unqualifiedCodeEntities = await _qualUnqualifiedCodeRepository.GetByIdsAsync(unqualifiedIds);

            List<ManuProductBadRecordReportViewDto> listDto = new List<ManuProductBadRecordReportViewDto>();
            foreach (var item in pagedInfo.Data)
            {
                var unqualifiedCodeEntitie = unqualifiedCodeEntities.FirstOrDefault(y => y.Id == item.UnqualifiedId);

                listDto.Add(new ManuProductBadRecordReportViewDto
                {
                    UnqualifiedId = item.UnqualifiedId,
                    Num = item.Num,
                    UnqualifiedCode = unqualifiedCodeEntitie?.UnqualifiedCode ?? "",
                    UnqualifiedCodeName = unqualifiedCodeEntitie?.UnqualifiedCodeName ?? ""
                });
            }

            return new PagedInfo<ManuProductBadRecordReportViewDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据查询条件导出参数数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<NioPushCollectionExportResultDto> ExprotAsync(BadRecordReportDto param)
        {
            var pagedQuery = param.ToQuery<ManuProductBadRecordReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;
            pagedQuery.PageSize = 1000;
            var pagedInfoList = await _manuProductBadRecordRepository.GetPagedInfoReportAsync(pagedQuery);

            var unqualifiedIds = pagedInfoList.Data.Select(x => x.UnqualifiedId).ToArray();
            var unqualifiedCodeEntities = await _qualUnqualifiedCodeRepository.GetByIdsAsync(unqualifiedIds);

            List<ManuProductBadRecordReportViewDto> dtos = new List<ManuProductBadRecordReportViewDto>();
            foreach (var item in pagedInfoList.Data)
            {
                var unqualifiedCodeEntitie = unqualifiedCodeEntities.FirstOrDefault(y => y.Id == item.UnqualifiedId);

                dtos.Add(new ManuProductBadRecordReportViewDto
                {
                    UnqualifiedId = item.UnqualifiedId,
                    Num = item.Num,
                    UnqualifiedCode = unqualifiedCodeEntitie?.UnqualifiedCode ?? "",
                    UnqualifiedCodeName = unqualifiedCodeEntitie?.UnqualifiedCodeName ?? ""
                });
            }


            // 实体到DTO转换 装载数据
            //var dtos = pagedInfo.Data.Select(s => s.ToModel<NioPushCollectionDto>());

            var pagedInfo = new PagedInfo<ManuProductBadRecordReportViewDto>(dtos, pagedInfoList.PageIndex, pagedInfoList.PageSize, pagedInfoList.TotalCount);

            //实体到DTO转换 装载数据
            List<ManuProductBadRecordReportViewExportDto> listDto = new();

            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                var filePathN = await _excelService.ExportAsync(listDto, _localizationService.GetResource("ManuProductBadRecordReport"), _localizationService.GetResource("ManuProductBadRecordReport"));
                //上传到文件服务器
                var uploadResultN = await _minioService.PutObjectAsync(filePathN);
                return new NioPushCollectionExportResultDto
                {
                    FileName = _localizationService.GetResource("ManuProductBadRecordReport"),
                    Path = uploadResultN.AbsoluteUrl,
                };
            }
            //对应的excel数值从这里开始
            foreach (var a in pagedInfo.Data)
            {
                var unqualifiedCodeEntitie = unqualifiedCodeEntities.FirstOrDefault(y => y.Id == a.UnqualifiedId);
                listDto.Add(new ManuProductBadRecordReportViewExportDto()
                {
                    UnqualifiedId = a.UnqualifiedId,
                    Num = a.Num,
                    UnqualifiedCode = unqualifiedCodeEntitie?.UnqualifiedCode ?? "",
                    UnqualifiedCodeName = unqualifiedCodeEntitie?.UnqualifiedCodeName ?? ""
                });
            }

            var filePath = await _excelService.ExportAsync(listDto, _localizationService.GetResource("ManuProductBadRecordReport"), _localizationService.GetResource("ManuProductBadRecordReport"));
            //上传到文件服务器
            var uploadResult = await _minioService.PutObjectAsync(filePath);
            return new NioPushCollectionExportResultDto
            {
                FileName = _localizationService.GetResource("ManuProductBadRecordReport"),
                Path = uploadResult.AbsoluteUrl,
            };

        }

        /// <summary>
        /// 根据查询条件获取不良报表分页数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<ManuProductBadRecordReportViewDto>> GetTopTenBadRecordAsync(BadRecordReportDto param)
        {
            var pagedQuery = param.ToQuery<ManuProductBadRecordReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;
            pagedQuery.PageIndex = 1;
            pagedQuery.PageSize = 10;

            var badRecordslist = await _manuProductBadRecordRepository.GetTopNumReportAsync(pagedQuery);

            var unqualifiedIds = badRecordslist.Select(x => x.UnqualifiedId).ToArray();
            var unqualifiedCodeEntities = await _qualUnqualifiedCodeRepository.GetByIdsAsync(unqualifiedIds);

            List<ManuProductBadRecordReportViewDto> listDto = new List<ManuProductBadRecordReportViewDto>();
            var sumNum = badRecordslist.ToList().Sum(x => x.Num);
            foreach (var item in badRecordslist)
            {
                var unqualifiedCodeEntitie = unqualifiedCodeEntities.FirstOrDefault((y => y.Id == item.UnqualifiedId));
                var manuProductBadRecordReport = new ManuProductBadRecordReportViewDto
                {
                    UnqualifiedId = item.UnqualifiedId,
                    Num = item.Num,
                    UnqualifiedCode = unqualifiedCodeEntitie?.UnqualifiedCode ?? "",
                    UnqualifiedCodeName = unqualifiedCodeEntitie?.UnqualifiedCodeName ?? ""
                };
                if (sumNum > 0)
                {
                    manuProductBadRecordReport.Percentage = Math.Round((((decimal)item.Num / sumNum)*100M), 6);
                }
                listDto.Add(manuProductBadRecordReport);
            }

            return listDto;
        }

        /// <summary>
        /// 根据查询条件获取不良日志报表分页数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuProductBadRecordLogReportViewDto>> GetLogPageListAsync(ManuProductBadRecordLogReportPagedQueryDto param)
        {
            var pagedQuery = param.ToQuery<ManuProductBadRecordLogReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _manuProductBadRecordRepository.GetPagedInfoLogReportAsync(pagedQuery);

            List<ManuProductBadRecordLogReportViewDto> listDto = new List<ManuProductBadRecordLogReportViewDto>();
            foreach (var item in pagedInfo.Data)
            {
                listDto.Add(item.ToModel<ManuProductBadRecordLogReportViewDto>());
            }

            return new PagedInfo<ManuProductBadRecordLogReportViewDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据查询条件导出参数数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<NioPushCollectionExportResultDto> LogExprotAsync(ManuProductBadRecordLogReportPagedQueryDto param)
        {
            var pagedQuery = param.ToQuery<ManuProductBadRecordLogReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;
            pagedQuery.PageSize = 1000;
            var pagedInfoList = await _manuProductBadRecordRepository.GetPagedInfoLogReportAsync(pagedQuery);

            List<ManuProductBadRecordLogReportViewDto> dtos = new List<ManuProductBadRecordLogReportViewDto>();
            foreach (var item in pagedInfoList.Data)
            {
                dtos.Add(item.ToModel<ManuProductBadRecordLogReportViewDto>());
            }


            // 实体到DTO转换 装载数据
            //var dtos = pagedInfo.Data.Select(s => s.ToModel<NioPushCollectionDto>());

            var pagedInfo = new PagedInfo<ManuProductBadRecordLogReportViewDto>(dtos, pagedInfoList.PageIndex, pagedInfoList.PageSize, pagedInfoList.TotalCount);

            //实体到DTO转换 装载数据
            List<ManuProductBadRecordLogReportViewExportDto> listDto = new();

            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                var filePathN = await _excelService.ExportAsync(listDto, _localizationService.GetResource("ManuProductBadRecordLogReport"), _localizationService.GetResource("ManuProductBadRecordLogReport"));
                //上传到文件服务器
                var uploadResultN = await _minioService.PutObjectAsync(filePathN);
                return new NioPushCollectionExportResultDto
                {
                    FileName = _localizationService.GetResource("ManuProductBadRecordLogReport"),
                    Path = uploadResultN.AbsoluteUrl,
                };
            }
            //对应的excel数值从这里开始
            foreach (var a in pagedInfo.Data)
            {
                listDto.Add(new ManuProductBadRecordLogReportViewExportDto()
                {
                    SFC = a.SFC,
                    MaterialCode = a.MaterialCode,
                    MaterialName = a.MaterialName,
                    OrderCode = a.OrderCode,
                    ProcedureCode = a.ProcedureCode,
                    ResCode = a.ResCode,
                    UnqualifiedCode = a.UnqualifiedCode,
                    UnqualifiedType = a.UnqualifiedType,
                    BadRecordStatus = a.BadRecordStatus,
                    Qty = a.Qty,
                    CreatedBy = a.CreatedBy,
                    CreatedOn = a.CreatedOn,

                });
            }

            var filePath = await _excelService.ExportAsync(listDto, _localizationService.GetResource("ManuProductBadRecordLogReport"), _localizationService.GetResource("ManuProductBadRecordLogReport"));
            //上传到文件服务器
            var uploadResult = await _minioService.PutObjectAsync(filePath);
            return new NioPushCollectionExportResultDto
            {
                FileName = _localizationService.GetResource("ManuProductBadRecordLogReport"),
                Path = uploadResult.AbsoluteUrl,
            };

        }

        /// <summary>
        /// 询不合格代码列表（不良报告日志）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductBadRecordLogReportResponseDto>> GetLogPageDetailListAsync(ManuProductBadRecordLogReportRequestDto request)
        {
            var manuProductNgRecordEntities = await _manuProductNgRecordRepository.GetEntitiesAsync(new ManuProducNGRecordQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                BadRecordId = request.BadRecordId,
                UnqualifiedId = request.UnqualifiedId
            });

            var qualUnqualifiedCodeEntities = await _qualUnqualifiedCodeRepository.GetByCodesAsync(new QualUnqualifiedCodeByCodesQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Codes = manuProductNgRecordEntities.Select(s => s.NGCode)
            });

            List<ManuProductBadRecordLogReportResponseDto> list = new();
            foreach (var item in manuProductNgRecordEntities)
            {
                var qualUnqualifiedCodeEntitiy = qualUnqualifiedCodeEntities.FirstOrDefault(f => f.UnqualifiedCode == item.NGCode);
                list.Add(new ManuProductBadRecordLogReportResponseDto
                {
                    Id = item.Id,
                    UnqualifiedCode = qualUnqualifiedCodeEntitiy?.UnqualifiedCode ?? item.NGCode,
                    UnqualifiedCodeName = qualUnqualifiedCodeEntitiy?.UnqualifiedCodeName ?? "-",
                    CreatedBy = item.CreatedBy,
                    CreatedOn = item.CreatedOn
                });
            }

            return list;
        }

    }
}
