using Dapper;
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcInfo.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Report;
using Hymson.Minio;
using Hymson.Utils;
using IdGen;

namespace Hymson.MES.Services.Services.Report
{
    /// <summary>
    /// 工单报告 服务
    /// </summary>
    public class WorkOrderControlReportService : IWorkOrderControlReportService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 表 仓储
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IManuSfcStepRepository _manuSfcStepRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;

        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IProcProcessRouteRepository _procProcessRouteRepository;
        private readonly IProcBomRepository _procBomRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IExcelService _excelService;
        private readonly IMinioService _minioService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procProcessRouteRepository"></param>
        /// <param name="procBomRepository"></param>
        /// <param name="localizationService"></param>
        /// <param name="excelService"></param>
        /// <param name="minioService"></param>
        public WorkOrderControlReportService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuSfcInfoRepository manuSfcInfoRepository, IManuSfcStepRepository manuSfcStepRepository,
            IProcProcedureRepository procProcedureRepository, IPlanWorkOrderRepository planWorkOrderRepository,
            IProcMaterialRepository procMaterialRepository, IProcProcessRouteRepository procProcessRouteRepository,
            IProcBomRepository procBomRepository, ILocalizationService localizationService,
            IExcelService excelService,
            IMinioService minioService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;

            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _procProcedureRepository = procProcedureRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procMaterialRepository = procMaterialRepository;
            _procProcessRouteRepository = procProcessRouteRepository;
            _procBomRepository = procBomRepository;

            _localizationService = localizationService;
            _excelService = excelService;
            _minioService = minioService;
        }

        /// <summary>
        /// 根据查询条件获取工单报告控制报表分页数据
        /// 优化: 不模糊查询，且通过关联ID查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WorkOrderControlReportViewDto>> GetWorkOrderControlPageListAsync(WorkOrderControlReportOptimizePagedQueryDto param)
        {
            var pagedQuery = param.ToQuery<PlanWorkOrderPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _planWorkOrderRepository.GetPagedInfoAsync(pagedQuery);

            List<WorkOrderControlReportViewDto> listDto = new();
            if (pagedInfo.Data.Any())
            {
                //// 查询bom
                //var procBomsTask = _procBomRepository.GetByIdsAsync(pagedInfo.Data.Select(x => x.ProductBOMId));

                // 查询物料
                var materialsTask = _procMaterialRepository.GetByIdsAsync(pagedInfo.Data.Select(x => x.ProductId));

                //// 查询工序
                //var procProceduresTask = _procProcedureRepository.GetByIdsAsync(pagedInfo.Data.Select(x => x.ProcedureId));

                var materials = await materialsTask;
                //var procProcedures = await procProceduresTask;

                foreach (var item in pagedInfo.Data)
                {
                    var material = materials.FirstOrDefault(x => x.Id == item.ProductId);

                    listDto.Add(new WorkOrderControlReportViewDto
                    {
                        Id = item.Id,
                        Status = item.Status,
                        Qty = item.Qty,
                        MaterialCode = material != null ? material.MaterialCode + "/" + material.Version : "",
                        WorkCenterId = item.WorkCenterCode ?? "",
                        OrderCode = item?.OrderCode ?? "",
                        Type = item?.Type,
                        PassDownQuantity = item?.PassDownQuantity ?? 0,
                        ProcessDownQuantity = item?.PassDownQuantity ?? 0 - item?.UnQualifiedQuantity ?? 0 - item?.FinishProductQuantity ?? 0,
                        UnQualifiedQuantity = item?.UnQualifiedQuantity ?? 0,
                        FinishProductQuantity = item?.FinishProductQuantity ?? 0,
                    });
                }

            }
            return new PagedInfo<WorkOrderControlReportViewDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 导出工单报告
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<WorkOrderControlExportResultDto> ExprotListAsync(WorkOrderControlReportOptimizePagedQueryDto param)
        {
            var pagedQuery = param.ToQuery<PlanWorkOrderPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;
            pagedQuery.PageSize = 10000;
            var pagedInfo = await _planWorkOrderRepository.GetPagedInfoAsync(pagedQuery);

            List<WorkOrderControlExportDto> listDto = new List<WorkOrderControlExportDto>();
            if (pagedInfo == null || pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                var filePathN = await _excelService.ExportAsync(listDto, _localizationService.GetResource("WorkOrderControl"), _localizationService.GetResource("WorkOrderControl"));
                //上传到文件服务器
                var uploadResultN = await _minioService.PutObjectAsync(filePathN);
                return new WorkOrderControlExportResultDto
                {
                    FileName = _localizationService.GetResource("WorkOrderControl"),
                    Path = uploadResultN.AbsoluteUrl,
                };
            }

            // 查询物料
            var materialsTask = _procMaterialRepository.GetByIdsAsync(pagedInfo.Data.Select(x => x.ProductId));
            var materials = await materialsTask;
            foreach (var item in pagedInfo.Data)
            {
                var material = materials.FirstOrDefault(x => x.Id == item.ProductId);
                listDto.Add(new WorkOrderControlExportDto
                {
                    Status = item.Status.GetDescription(),
                    Qty = item.Qty,
                    MaterialCode = material != null ? material.MaterialCode + "/" + material.Version : "",
                    WorkCenterId = item.WorkCenterCode ?? "",
                    OrderCode = item?.OrderCode ?? "",
                    Type = item?.Type.GetDescription(),
                    PassDownQuantity = item?.PassDownQuantity ?? 0,
                    ProcessDownQuantity = item?.PassDownQuantity ?? 0 - item?.UnQualifiedQuantity ?? 0 - item?.FinishProductQuantity ?? 0,
                    UnQualifiedQuantity = item?.UnQualifiedQuantity ?? 0,
                    FinishProductQuantity = item?.FinishProductQuantity ?? 0,
                });
            }

            var filePath = await _excelService.ExportAsync(listDto, _localizationService.GetResource("WorkOrderControl"), _localizationService.GetResource("WorkOrderControl"));
            //上传到文件服务器
            var uploadResult = await _minioService.PutObjectAsync(filePath);
            return new WorkOrderControlExportResultDto
            {
                FileName = _localizationService.GetResource("WorkOrderControl"),
                Path = uploadResult.AbsoluteUrl,
            };
        }
    }
}
