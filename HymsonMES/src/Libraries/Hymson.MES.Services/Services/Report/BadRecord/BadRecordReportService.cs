using AutoMapper.Execution;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Services.Dtos.Report;

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
            IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;

            _manuProductBadRecordRepository = manuProductBadRecordRepository;
            _manuProductNgRecordRepository = manuProductNgRecordRepository;
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
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
