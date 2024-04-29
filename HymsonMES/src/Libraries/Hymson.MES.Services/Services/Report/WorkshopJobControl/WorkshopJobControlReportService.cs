using Dapper;
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcInfo.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Report;

namespace Hymson.MES.Services.Services.Report
{
    /// <summary>
    /// 不良记录报表 服务
    /// </summary>
    public class WorkshopJobControlReportService : IWorkshopJobControlReportService
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procProcessRouteRepository"></param>
        /// <param name="procBomRepository"></param>

        public WorkshopJobControlReportService(ICurrentUser currentUser, ICurrentSite currentSite, IManuSfcInfoRepository manuSfcInfoRepository, IManuSfcStepRepository manuSfcStepRepository, IProcProcedureRepository procProcedureRepository, IPlanWorkOrderRepository planWorkOrderRepository, IProcMaterialRepository procMaterialRepository, IProcProcessRouteRepository procProcessRouteRepository, IProcBomRepository procBomRepository)
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
        }

        ///// <summary>
        ///// 根据查询条件获取车间作业控制报表分页数据
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //public async Task<PagedInfo<WorkshopJobControlReportViewDto>> GetWorkshopJobControlPageListAsync(WorkshopJobControlReportPagedQueryDto param)
        //{
        //    var pagedQuery = param.ToQuery<WorkshopJobControlReportPagedQuery>();
        //    pagedQuery.SiteId = _currentSite.SiteId;
        //    var pagedInfo = await _manuSfcInfoRepository.GetPagedInfoWorkshopJobControlReportAsync(pagedQuery);

        //    List<WorkshopJobControlReportViewDto> listDto = new List<WorkshopJobControlReportViewDto>();
        //    foreach (var item in pagedInfo.Data)
        //    {
        //        listDto.Add(item.ToModel<WorkshopJobControlReportViewDto>());
        //    }

        //    return new PagedInfo<WorkshopJobControlReportViewDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        //}

        /// <summary>
        /// 根据查询条件获取车间作业控制报表分页数据
        /// 优化: 不模糊查询，且通过关联ID查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WorkshopJobControlReportViewDto>> GetWorkshopJobControlPageListAsync(WorkshopJobControlReportOptimizePagedQueryDto param)
        {
            var pagedQuery = param.ToQuery<WorkshopJobControlReportOptimizePagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _manuSfcInfoRepository.GetPagedInfoWorkshopJobControlReportOptimizeAsync(pagedQuery);

            List<WorkshopJobControlReportViewDto> listDto = new();
            if (pagedInfo.Data.Any())
            {
                // 查询工单
                var workOrders = await _planWorkOrderRepository.GetByIdsAsync(pagedInfo.Data.Select(x => x.WorkOrderId));

                // 查询bom
                var procBomsTask = _procBomRepository.GetByIdsAsync(pagedInfo.Data.Select(x => x.ProductBOMId));

                // 查询物料
                var materialsTask = _procMaterialRepository.GetByIdsAsync(pagedInfo.Data.Select(x => x.ProductId));

                // 查询工序
                var procProceduresTask = _procProcedureRepository.GetByIdsAsync(pagedInfo.Data.Select(x => x.ProcedureId));

                var procBoms = await procBomsTask;
                var materials = await materialsTask;
                var procProcedures = await procProceduresTask;

                foreach (var item in pagedInfo.Data)
                {
                    var material = materials.FirstOrDefault(x => x.Id == item.ProductId);
                    var workOrder = workOrders.FirstOrDefault(x => x.Id == item.WorkOrderId);
                    var procedure = procProcedures.FirstOrDefault(x => x.Id == item.ProcedureId);
                    var bom = procBoms.FirstOrDefault(x => x.Id == item.ProductBOMId);

                    listDto.Add(new WorkshopJobControlReportViewDto
                    {
                        SFC = item.SFC,
                        SFCStatus = item.SFCStatus,
                        SFCProduceStatus = item.SFCStatus,
                        Qty = item.Qty,
                        MaterialCodeVersion = material != null ? material.MaterialCode + "/" + material.Version : "",
                        MaterialName = material?.MaterialName ?? "",
                        OrderCode = workOrder?.OrderCode ?? "",
                        OrderType = workOrder?.Type,
                        ProcedureCode = procedure?.Code ?? "",
                        ProcedureName = procedure?.Name ?? "",
                        BomCodeVersion = bom != null ? bom.BomCode + "/" + bom.Version : "",
                        BomName = bom != null ? bom.BomName : ""
                    });
                }

            }
            return new PagedInfo<WorkshopJobControlReportViewDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 获取SFC的车间作业控制步骤
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<WorkshopJobControlStepReportDto> GetSfcInOutInfoAsync(string sfc)
        {
            var responseDto = new WorkshopJobControlStepReportDto() { SFC = sfc };

            var sfcInfoEntity = await _manuSfcInfoRepository.GetUsedBySFCAsync(sfc)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES18106)).WithData("sfc", sfc);

            #region 查询基础数据
            // 查询工单信息
            var workOrder = await _planWorkOrderRepository.GetByIdAsync(sfcInfoEntity.WorkOrderId ?? 0)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES18102)).WithData("sfc", sfc);

            // 查询物料信息
            var materialEntity = await _procMaterialRepository.GetByIdAsync(sfcInfoEntity.ProductId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES18103)).WithData("sfc", sfc);

            // 查询工艺路线
            var processRouteEntity = await _procProcessRouteRepository.GetByIdAsync(workOrder.ProcessRouteId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES18104)).WithData("sfc", sfc);

            // 查询Bom
            var bomEntity = await _procBomRepository.GetByIdAsync(workOrder.ProductBOMId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES18105)).WithData("sfc", sfc);

            responseDto.OrderCode = workOrder.OrderCode;
            responseDto.MaterialCodrNameVersion = materialEntity.MaterialCode + "/" + materialEntity.MaterialName + "/" + materialEntity.Version;
            responseDto.ProcessRouteCodeNameVersion = processRouteEntity.Code + "/" + processRouteEntity.Name + "/" + processRouteEntity.Version;
            responseDto.ProcBomCodeNameVersion = bomEntity.BomCode + "/" + bomEntity.BomName + "/" + bomEntity.Version;
            #endregion

            // 查询进出站步骤
            var sfcSteps = await _manuSfcStepRepository.GetInOutStationStepsBySFCAsync(new EntityBySFCQuery() { SiteId = _currentSite.SiteId ?? 0, SFC = sfc });
            if (sfcSteps == null || !sfcSteps.Any()) return responseDto;

            // 查询工单信息
            var workOrders = await _planWorkOrderRepository.GetByIdsAsync(sfcSteps.Select(x => x.WorkOrderId).Distinct().ToArray());

            // 入站
            var inSfcSteps = sfcSteps.Where(x => x.Operatetype == Core.Enums.Manufacture.ManuSfcStepTypeEnum.InStock).OrderBy(x => x.CreatedOn);

            // 出站
            var outSfcSteps = sfcSteps.Where(x => x.Operatetype == Core.Enums.Manufacture.ManuSfcStepTypeEnum.OutStock);

            // 如果无步骤信息
            if (inSfcSteps == null || !inSfcSteps.Any()) return responseDto;

            var procedureIds = inSfcSteps.Where(w => w.ProcedureId.HasValue).Select(x => x.ProcedureId!.Value).Distinct();
            var procedureEntities = await _procProcedureRepository.GetByIdsAsync(procedureIds);

            List<WorkshopJobControlInOutSteptDto> stepDtos = new();

            var inSfcStepList = inSfcSteps.ToList();
            for (int i = 0; i < inSfcStepList.Count; i++)
            {
                // 查找当前出站时间
                var currentStep = inSfcStepList[i];
                ManuSfcStepEntity? nextStep = null;
                ManuSfcStepEntity? outStep = null;

                // 是否有下一个进站
                if (i + 1 < inSfcStepList.Count)
                {
                    nextStep = inSfcStepList[i + 1];
                    // 查找出站时间
                    outStep = outSfcSteps.FirstOrDefault(x => currentStep.Id < x.Id && x.Id < nextStep.Id);
                    // outStep = outSfcSteps.FirstOrDefault(x => currentStep.CreatedOn < x.CreatedOn && x.CreatedOn < nextStep.CreatedOn);
                }
                else
                {
                    // 查找出站时间
                    // outStep = outSfcSteps.FirstOrDefault(x => currentStep.CreatedOn < x.CreatedOn);
                    outStep = outSfcSteps.FirstOrDefault(x => currentStep.Id < x.Id);
                }

                stepDtos.Add(new WorkshopJobControlInOutSteptDto
                {
                    Id = currentStep.Id,
                    WorkOrderCode = workOrders.FirstOrDefault(x => x.Id == currentStep.WorkOrderId)?.OrderCode ?? string.Empty,
                    ProcedureCode = procedureEntities.FirstOrDefault(x => x.Id == currentStep.ProcedureId)?.Code ?? string.Empty,
                    Status = nextStep != null || outStep != null ? SfcInOutStatusEnum.Finished : SfcInOutStatusEnum.Activity,
                    InDateTime = currentStep.CreatedOn,
                    OutDatetTime = outStep?.CreatedOn
                });
            }

            // 对 stepDtos 进行排序
            responseDto.WorkshopJobControlInOutSteptDtos = stepDtos.OrderBy(o => o.Id);
            return responseDto;
        }

        /// <summary>
        /// 根据SFC分页获取条码步骤信息
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcStepBySfcViewDto>> GetSFCStepsBySFCPageListAsync(ManuSfcStepBySfcPagedQueryDto queryParam)
        {
            var pagedQuery = queryParam.ToQuery<ManuSfcStepBySfcPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId.Value;

            if (string.IsNullOrEmpty(pagedQuery.SFC))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18110));
            }

            var pagedInfo = await _manuSfcStepRepository.GetPagedInfoBySFCAsync(pagedQuery);

            List<ManuSfcStepBySfcViewDto> listDto = new List<ManuSfcStepBySfcViewDto>();

            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                return new PagedInfo<ManuSfcStepBySfcViewDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
            }

            var materialIds = pagedInfo.Data.Select(x => x.ProductId).Distinct().ToArray();
            var materials = await _procMaterialRepository.GetByIdsAsync(materialIds);

            var procedureIds = pagedInfo.Data.Select(x => x.ProcedureId == null ? 0 : x.ProcedureId.Value).Distinct().ToArray();
            var procedures = procedureIds != null && procedureIds.Any() ? await _procProcedureRepository.GetByIdsAsync(procedureIds) : null;

            var workOrderIds = pagedInfo.Data.Select(x => x.WorkOrderId).Distinct().ToArray();
            var workOrders = await _planWorkOrderRepository.GetByIdsAsync(workOrderIds);

            foreach (var item in pagedInfo.Data)
            {
                var material = materials != null && materials.Any() ? materials.FirstOrDefault(x => x.Id == item.ProductId) : null;
                var procedure = procedures != null && procedures.Any() ? procedures.FirstOrDefault(x => x.Id == item.ProcedureId) : null;
                var workOrder = workOrders != null && workOrders.Any() ? workOrders.FirstOrDefault(x => x.Id == item.WorkOrderId) : null;

                listDto.Add(new ManuSfcStepBySfcViewDto()
                {
                    Id = item.Id,
                    SFC = item.SFC,
                    Operatetype = item.Operatetype,
                    CreatedOn = item.CreatedOn,
                    MaterialCodeVersion = material != null ? material.MaterialCode + "/" + material.Version : "",
                    MaterialName = material != null ? material.MaterialName : "",
                    ProcedureCode = procedure != null ? procedure.Code : "",
                    ProcedureName = procedure != null ? procedure.Name : "",
                    OrderCode = workOrder != null ? workOrder.OrderCode : ""
                });
            }

            // 因为job合并执行的时候，时间会一样，所以加上类型排序
            var dtoOrdered = listDto.OrderBy(o => o.Id).AsEnumerable();
            return new PagedInfo<ManuSfcStepBySfcViewDto>(dtoOrdered, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }
    }
}
