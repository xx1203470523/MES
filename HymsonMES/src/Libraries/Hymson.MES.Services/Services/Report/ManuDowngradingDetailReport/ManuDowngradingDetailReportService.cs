using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants.Report;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Report;
using Hymson.MES.Data.Repositories.Report.Query;
using Hymson.MES.Services.Dtos.Report;
using Hymson.Minio;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Report
{
    /// <summary>
    /// 服务（降级品明细报表） 
    /// </summary>
    public class ManuDowngradingDetailReportService : IManuDowngradingDetailReportService
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        private readonly ICurrentUser _currentUser;
        /// <summary>
        /// 当前站点
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 仓储接口（降级品明细报表）
        /// </summary>
        private readonly IManuDowngradingDetailReportRepository _manuDowngradingDetailReportRepository;

        /// <summary>
        /// 工作中心仓储
        /// </summary>
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;

        /// <summary>
        /// 工序仓储
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 产品仓储
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 降级规则仓储
        /// </summary>
        private readonly IManuDowngradingRuleRepository _manuDowngradingRuleRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IExcelService _excelService;
        private readonly IMinioService _minioService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuDowngradingDetailReportRepository"></param>
        /// <param name="inteWorkCenterRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="manuDowngradingRuleRepository"></param>
        /// <param name="localizationService"></param>
        /// <param name="excelService"></param>
        /// <param name="minioService"></param>
        public ManuDowngradingDetailReportService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuDowngradingDetailReportRepository manuDowngradingDetailReportRepository,
            IInteWorkCenterRepository inteWorkCenterRepository,
            IProcProcedureRepository procProcedureRepository,
            IProcMaterialRepository procMaterialRepository,
            IManuDowngradingRuleRepository manuDowngradingRuleRepository,
            ILocalizationService localizationService,
            IExcelService excelService,
            IMinioService minioService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuDowngradingDetailReportRepository = manuDowngradingDetailReportRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _procProcedureRepository = procProcedureRepository;
            _procMaterialRepository = procMaterialRepository;
            _manuDowngradingRuleRepository = manuDowngradingRuleRepository;

            _localizationService = localizationService;
            _excelService = excelService;
            _minioService = minioService;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuDowngradingDetailReportDto>> GetPagedListAsync(ManuDowngradingDetailReportPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ManuDowngradingDetailReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _manuDowngradingDetailReportRepository.GetPagedInfoAsync(pagedQuery);

            List<ManuDowngradingDetailReportDto> listDto = new();
            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                return new PagedInfo<ManuDowngradingDetailReportDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
            }

            //工序
            var procedureIds = pagedInfo.Data.Select(x => x.ProcedureId.GetValueOrDefault()).Distinct().ToArray();
            var procedureInfosTask = _procProcedureRepository.GetByIdsAsync(procedureIds);
            //产品
            var productIds = pagedInfo.Data.Select(x => x.ProductId).Distinct().ToArray();
            var productInfosTask = _procMaterialRepository.GetByIdsAsync(productIds);
            //工作中心
            var workCenterIds = pagedInfo.Data.Select(x => x.WorkCenterId.GetValueOrDefault()).Distinct().ToArray();
            var workCenterInfosTask = _inteWorkCenterRepository.GetByIdsAsync(workCenterIds);

            var procedureInfos = await procedureInfosTask;
            var productInfos = await productInfosTask;
            var workCenterInfos = await workCenterInfosTask;

            foreach (var item in pagedInfo.Data)
            {
                var procedureInfo = procedureInfos.FirstOrDefault(y => y.Id == item.ProcedureId);
                var productInfo = productInfos.FirstOrDefault(y => y.Id == item.ProductId);
                var workCenterInfo = workCenterInfos.FirstOrDefault(y => y.Id == item.WorkCenterId);

                var downgradingRuleInfo = await _manuDowngradingRuleRepository.GetByCodeAsync(new ManuDowngradingRuleCodeQuery() { Code = item.Grade ?? "", SiteId = (long)_currentSite.SiteId! });
                //添加数据
                listDto.Add(new ManuDowngradingDetailReportDto
                {
                    WorkCenterCode = workCenterInfo?.Code ?? "",
                    SFC = item.SFC,
                    ProductCode = productInfo?.MaterialCode ?? "",
                    ProductName = productInfo?.MaterialName ?? "",
                    DowngradingCode = item.Grade,
                    DowngradingName = downgradingRuleInfo?.Name ?? "",
                    DowngradingRemark = item.Remark,
                    UpdatedBy = item.EntryPersonnel,
                    UpdatedOn = item.EntryTime,
                    OrderCode = item.OrderCode,
                    OrderType = item.OrderType,
                    ProcedureCode = procedureInfo?.Code ?? "",
                    ProcedureName = procedureInfo?.Name ?? "",
                });
            }

            return new PagedInfo<ManuDowngradingDetailReportDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 导出查询列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ManuDowngradingDetailExportResultDto> ExprotListAsync(ManuDowngradingDetailReportPagedQueryDto param)
        {
            var pagedQuery = param.ToQuery<ManuDowngradingDetailReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            pagedQuery.PageSize = ReportExport.PageSize;
            var pagedInfo = await _manuDowngradingDetailReportRepository.GetPagedInfoAsync(pagedQuery);

            List<ManuDowngradingDetailExportDto> listDto = new List<ManuDowngradingDetailExportDto>();
            if (pagedInfo == null || pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                var filePathN = await _excelService.ExportAsync(listDto, _localizationService.GetResource("ManuDowngradingDetailReport"), _localizationService.GetResource("ManuDowngradingDetailReport"));
                //上传到文件服务器
                var uploadResultN = await _minioService.PutObjectAsync(filePathN);
                return new ManuDowngradingDetailExportResultDto
                {
                    FileName = _localizationService.GetResource("ManuDowngradingDetailReport"),
                    Path = uploadResultN.AbsoluteUrl,
                };
            }

            //工序
            var procedureIds = pagedInfo.Data.Select(x => x.ProcedureId.GetValueOrDefault()).Distinct().ToArray();
            var procedureInfosTask = _procProcedureRepository.GetByIdsAsync(procedureIds);
            //产品
            var productIds = pagedInfo.Data.Select(x => x.ProductId).Distinct().ToArray();
            var productInfosTask = _procMaterialRepository.GetByIdsAsync(productIds);
            //工作中心
            var workCenterIds = pagedInfo.Data.Select(x => x.WorkCenterId.GetValueOrDefault()).Distinct().ToArray();
            var workCenterInfosTask = _inteWorkCenterRepository.GetByIdsAsync(workCenterIds);

            var procedureInfos = await procedureInfosTask;
            var productInfos = await productInfosTask;
            var workCenterInfos = await workCenterInfosTask;

            foreach (var item in pagedInfo.Data)
            {
                var procedureInfo = procedureInfos.FirstOrDefault(y => y.Id == item.ProcedureId);
                var productInfo = productInfos.FirstOrDefault(y => y.Id == item.ProductId);
                var workCenterInfo = workCenterInfos.FirstOrDefault(y => y.Id == item.WorkCenterId);

                var downgradingRuleInfo = await _manuDowngradingRuleRepository.GetByCodeAsync(new ManuDowngradingRuleCodeQuery() { Code = item.Grade ?? "", SiteId = (long)_currentSite.SiteId! });
                //添加数据
                listDto.Add(new ManuDowngradingDetailExportDto
                {
                    WorkCenterCode = workCenterInfo?.Code ?? "",
                    SFC = item.SFC,
                    ProductCode = productInfo?.MaterialCode ?? "",
                    ProductName = productInfo?.MaterialName ?? "",
                    DowngradingCode = item.Grade,
                    DowngradingName = downgradingRuleInfo?.Name ?? "",
                    DowngradingRemark = item.Remark,
                    UpdatedBy = item.EntryPersonnel,
                    UpdatedOn = item.EntryTime,
                    OrderCode = item.OrderCode,
                    OrderType = item.OrderType.GetDescription(),
                    ProcedureCode = procedureInfo?.Code ?? "",
                    ProcedureName = procedureInfo?.Name ?? "",
                });
            }

            var filePath = await _excelService.ExportAsync(listDto, _localizationService.GetResource("ManuDowngradingDetailReport"), _localizationService.GetResource("ManuDowngradingDetailReport"));
            //上传到文件服务器
            var uploadResult = await _minioService.PutObjectAsync(filePath);
            return new ManuDowngradingDetailExportResultDto
            {
                FileName = _localizationService.GetResource("ManuDowngradingDetailReport"),
                Path = uploadResult.AbsoluteUrl,
            };
        }
    }
}
