using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Marking;
using Hymson.MES.Data.Repositories.Marking.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Marking;
using Hymson.MES.Services.Dtos.Report;
using Hymson.Minio;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Marking
{
    /// <summary>
    /// 服务（Marking拦截汇总表） 
    /// </summary>
    public class MarkingRecordReportService : IMarkingRecordReportService
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
        /// 仓储接口（Marking拦截汇总表）
        /// </summary>
        private readonly IMarkingRecordReportRepository _markingInterceptReportRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IEquEquipmentRepository _equEquipmentRepository;
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IExcelService _excelService;
        private readonly IMinioService _minioService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="markingInterceptReportRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="equEquipmentRepository"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="inteWorkCenterRepository"></param>
        /// <param name="localizationService"></param>
        /// <param name="excelService"></param>
        /// <param name="minioService"></param>
        public MarkingRecordReportService(ICurrentUser currentUser, ICurrentSite currentSite,
            IMarkingRecordReportRepository markingInterceptReportRepository,
            IProcMaterialRepository procMaterialRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IProcProcedureRepository procProcedureRepository,
            IEquEquipmentRepository equEquipmentRepository,
            IProcResourceRepository procResourceRepository,
            IInteWorkCenterRepository inteWorkCenterRepository, 
            ILocalizationService localizationService,
            IExcelService excelService,
            IMinioService minioService)

        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procMaterialRepository = procMaterialRepository;
            _markingInterceptReportRepository = markingInterceptReportRepository;
            _procProcedureRepository = procProcedureRepository;
            _equEquipmentRepository = equEquipmentRepository;
            _procResourceRepository = procResourceRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;

            _localizationService = localizationService;
            _excelService = excelService;
            _minioService = minioService;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<MarkingRecordReportDto>> GetPagedListAsync(MarkingInterceptReportPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<MarkingReportReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _markingInterceptReportRepository.GetPagedInfoAsync(pagedQuery);
            List<MarkingRecordReportDto> listDto = new List<MarkingRecordReportDto>();
            var pagedInfoData = pagedInfo.Data;
            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                return new PagedInfo<MarkingRecordReportDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
            }
            var materialIds = pagedInfoData.Select(x => x.ProductId).Distinct().ToList();
            var procMaterialInfos = await _procMaterialRepository.GetByIdsAsync(materialIds);

            var workOrderIds = pagedInfoData.Select(x => x.WorkOrderId).Distinct().ToArray();
            var workOrderInfos = await _planWorkOrderRepository.GetByIdsAsync(workOrderIds);

            var procedureIds = pagedInfoData.Select(x => x.FindProcedureId).Distinct().ToList();
            var interceptPrecedureIds = pagedInfoData.Select(x => x.AppointInterceptProcedureId).Distinct().ToList();
            if (interceptPrecedureIds.Any())
            {
                procedureIds.AddRange(interceptPrecedureIds);
            }
            var interceptProducedureIds = pagedInfoData.Select(x => x.InterceptProcedureId).Distinct().ToList();
            if (interceptProducedureIds.Any())
            {
                procedureIds.AddRange(interceptProducedureIds);
            }
            var procedureInfos = await _procProcedureRepository.GetByIdsAsync(procedureIds.Distinct());

            var interceptEquipmentIds = pagedInfoData.Select(x => x.InterceptEquipmentId).Distinct().ToList();
            var interceptEquipmentInfos = await _equEquipmentRepository.GetByIdAsync(interceptEquipmentIds);

            var resourceIds = pagedInfoData.Select(x => x.ResourceId).Distinct().ToArray();
            var resourceInfos = await _procResourceRepository.GetListByIdsAsync(resourceIds);

            var interceptWorkCenterIds = workOrderInfos.Select(x => x.WorkCenterId.GetValueOrDefault()).Distinct().ToArray();
            var workCenterInfos = await _inteWorkCenterRepository.GetByIdsAsync(interceptWorkCenterIds);

            foreach (var item in pagedInfoData)
            {
                var procMaterialInfo = procMaterialInfos.FirstOrDefault(x => x.Id == item.ProductId);
                var workOrderInfo = workOrderInfos.FirstOrDefault(x => x.Id == item.WorkOrderId);
                var findProcedureInfo = procedureInfos.FirstOrDefault(x => x.Id == item.FindProcedureId);
                var appointInterceptInfo = procedureInfos.FirstOrDefault(x => x.Id == item.AppointInterceptProcedureId);
                var interceptProcedureInfo = procedureInfos.FirstOrDefault(x => x.Id == item.InterceptProcedureId);
                var interceptEquipmentInfo = interceptEquipmentInfos.FirstOrDefault(x => x.Id == item.InterceptEquipmentId);
                var resourceInfo = resourceInfos.FirstOrDefault(x => x.Id == item.ResourceId);
                var workCenterInfo = workCenterInfos.FirstOrDefault(x => x.Id == (workOrderInfo?.WorkCenterId ?? 0));

                // 实体到DTO转换 装载数据
                listDto.Add(new MarkingRecordReportDto
                {
                    SFC = item.SFC,
                    MaterialCode = procMaterialInfo?.MaterialCode ?? "",
                    MaterialName = procMaterialInfo?.MaterialName ?? "",
                    OrderCode = workOrderInfo?.OrderCode ?? "",
                    WorkCenterName = workCenterInfo?.Name ?? "",
                    OrderType = workOrderInfo?.Type,
                    FindProcedureName = findProcedureInfo?.Name ?? "",
                    AppointInterceptProcedureName = appointInterceptInfo?.Name ?? "",
                    InterceptProcedureName = interceptProcedureInfo?.Name ?? "",
                    InterceptEquipmentName = interceptEquipmentInfo?.EquipmentName ?? "",
                    ResourceName = resourceInfo?.ResName ?? "",
                    UnqualifiedCode = item.UnqualifiedCode,
                    UnqualifiedCodeName = item.UnqualifiedCodeName,
                    UnqualifiedStatus = item.MarkingStatus == 1 ? "1" : "2",
                    UnqualifiedType = item.Type,
                    Qty = item.Qty,
                    InterceptOn = item.InterceptOn,
                    MarkingCreatedBy = item.MarkingCreatedBy,
                    MarkingCreatedOn = item.MarkingCreatedOn,
                    MarkingClosedBy = item.MarkingStatus == 0 ? item.MarkingClosedBy : null,
                    MarkingClosedOn = item.MarkingStatus == 0 ? item.MarkingClosedOn : null,
                }); ;
            }
            return new PagedInfo<MarkingRecordReportDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 导出查询数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<MarkingRecordExportResultDto> ExprotListAsync(MarkingInterceptReportPagedQueryDto param)
        {
            var pagedQuery = param.ToQuery<MarkingReportReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;
            pagedQuery.PageSize = 10000;
            var pagedInfo = await _markingInterceptReportRepository.GetPagedInfoAsync(pagedQuery);

            List<MarkingRecordExportDto> listDto = new List<MarkingRecordExportDto>();
            var pagedInfoData = pagedInfo.Data;
            if (pagedInfo == null || pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                var filePathN = await _excelService.ExportAsync(listDto, _localizationService.GetResource("MarkingRecordExport"), _localizationService.GetResource("MarkingRecordExport"));
                //上传到文件服务器
                var uploadResultN = await _minioService.PutObjectAsync(filePathN);
                return new MarkingRecordExportResultDto
                {
                    FileName = _localizationService.GetResource("MarkingRecordExport"),
                    Path = uploadResultN.AbsoluteUrl,
                };
            }

            var materialIds = pagedInfoData.Select(x => x.ProductId).Distinct().ToList();
            var procMaterialInfos = await _procMaterialRepository.GetByIdsAsync(materialIds);

            var workOrderIds = pagedInfoData.Select(x => x.WorkOrderId).Distinct().ToArray();
            var workOrderInfos = await _planWorkOrderRepository.GetByIdsAsync(workOrderIds);

            var procedureIds = pagedInfoData.Select(x => x.FindProcedureId).Distinct().ToList();
            var interceptPrecedureIds = pagedInfoData.Select(x => x.AppointInterceptProcedureId).Distinct().ToList();
            if (interceptPrecedureIds.Any())
            {
                procedureIds.AddRange(interceptPrecedureIds);
            }
            var interceptProducedureIds = pagedInfoData.Select(x => x.InterceptProcedureId).Distinct().ToList();
            if (interceptProducedureIds.Any())
            {
                procedureIds.AddRange(interceptProducedureIds);
            }
            var procedureInfos = await _procProcedureRepository.GetByIdsAsync(procedureIds.Distinct());

            var interceptEquipmentIds = pagedInfoData.Select(x => x.InterceptEquipmentId).Distinct().ToList();
            var interceptEquipmentInfos = await _equEquipmentRepository.GetByIdAsync(interceptEquipmentIds);

            var resourceIds = pagedInfoData.Select(x => x.ResourceId).Distinct().ToArray();
            var resourceInfos = await _procResourceRepository.GetListByIdsAsync(resourceIds);

            var interceptWorkCenterIds = workOrderInfos?.Select(x => x.WorkCenterId.GetValueOrDefault()).Distinct().ToArray();
            var workCenterInfos = await _inteWorkCenterRepository.GetByIdsAsync(interceptWorkCenterIds);

            foreach (var item in pagedInfoData)
            {
                var procMaterialInfo = procMaterialInfos.FirstOrDefault(x => x.Id == item.ProductId);
                var workOrderInfo = workOrderInfos?.FirstOrDefault(x => x.Id == item.WorkOrderId);
                var findProcedureInfo = procedureInfos.FirstOrDefault(x => x.Id == item.FindProcedureId);
                var appointInterceptInfo = procedureInfos.FirstOrDefault(x => x.Id == item.AppointInterceptProcedureId);
                var interceptProcedureInfo = procedureInfos.FirstOrDefault(x => x.Id == item.InterceptProcedureId);
                var interceptEquipmentInfo = interceptEquipmentInfos.FirstOrDefault(x => x.Id == item.InterceptEquipmentId);
                var resourceInfo = resourceInfos.FirstOrDefault(x => x.Id == item.ResourceId);
                var workCenterInfo = workCenterInfos.FirstOrDefault(x => x.Id>0);

                // 实体到DTO转换 装载数据
                listDto.Add(new MarkingRecordExportDto
                {
                    SFC = item.SFC,
                    MaterialCode = procMaterialInfo?.MaterialCode ?? "",
                    MaterialName = procMaterialInfo?.MaterialName ?? "",
                    OrderCode = workOrderInfo?.OrderCode ?? "",
                    WorkCenterName = workCenterInfo?.Name ?? "",
                    OrderType = workOrderInfo!.Type.GetDescription(),
                    FindProcedureName = findProcedureInfo?.Name ?? "",
                    AppointInterceptProcedureName = appointInterceptInfo?.Name ?? "",
                    InterceptProcedureName = interceptProcedureInfo?.Name ?? "",
                    InterceptEquipmentName = interceptEquipmentInfo?.EquipmentName ?? "",
                    ResourceName = resourceInfo?.ResName ?? "",
                    UnqualifiedCode = item.UnqualifiedCode,
                    UnqualifiedCodeName = item.UnqualifiedCodeName,
                    UnqualifiedStatus = item.Status,
                    UnqualifiedType = item.Type.GetDescription(),
                    Qty = item.Qty,
                    InterceptOn = item.InterceptOn,
                    MarkingCreatedBy = item.MarkingCreatedBy,
                    MarkingCreatedOn = item.MarkingCreatedOn,
                    MarkingClosedBy = item.MarkingStatus == 0 ? item.MarkingClosedBy : null,
                    MarkingClosedOn = item.MarkingStatus == 0 ? item.MarkingClosedOn : null,
                }); ;
            }

            var filePath = await _excelService.ExportAsync(listDto, _localizationService.GetResource("MarkingRecordExport"), _localizationService.GetResource("MarkingRecordExport"));
            //上传到文件服务器
            var uploadResult = await _minioService.PutObjectAsync(filePath);
            return new MarkingRecordExportResultDto
            {
                FileName = _localizationService.GetResource("MarkingRecordExport"),
                Path = uploadResult.AbsoluteUrl,
            };
        }
    }
}
