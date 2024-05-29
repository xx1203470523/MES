using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Attribute.Manufacture;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcInfo.Query;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Data.Repositories.Parameter;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Services.Dtos.Report;
using Minio.DataModel;

namespace Hymson.MES.Services.Services.Report
{
    /// <summary>
    /// 不良记录报表 服务
    /// </summary>
    public class WorkshopJobControlReportService : IWorkshopJobControlReportService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        private readonly ILocalizationService _localizationService;

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
        private readonly IManuSfcRepository _manuSfcRepository;

        private readonly IProcResourceRepository _procResourceRepository;

        private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;
        private readonly IManuDowngradingRecordRepository _manuDowngradingRecordRepository;
        /// <summary>
        /// 产品不良录入 仓储
        /// </summary>
        private readonly IManuProductBadRecordRepository _manuProductBadRecordRepository;

        private readonly IManuBarCodeRelationRepository _manuBarCodeRelationRepository;

        private readonly IManuFacePlateRepairRepository _manuFacePlateRepairRepository;

        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        private readonly IManuProductParameterRepository _manuProductParameterRepository;

        private readonly IProcParameterRepository _procParameterRepository;

        public WorkshopJobControlReportService(ICurrentUser currentUser, ICurrentSite currentSite, ILocalizationService localizationService, IManuSfcInfoRepository manuSfcInfoRepository, IManuSfcStepRepository manuSfcStepRepository, IProcProcedureRepository procProcedureRepository, IPlanWorkOrderRepository planWorkOrderRepository, IProcMaterialRepository procMaterialRepository, IProcProcessRouteRepository procProcessRouteRepository, IProcBomRepository procBomRepository, IManuSfcRepository manuSfcRepository, IProcResourceRepository procResourceRepository, IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository, IManuDowngradingRecordRepository manuDowngradingRecordRepository, IManuProductBadRecordRepository manuProductBadRecordRepository, IManuBarCodeRelationRepository manuBarCodeRelationRepository, IManuFacePlateRepairRepository manuFacePlateRepairRepository, IManuSfcProduceRepository manuSfcProduceRepository, IManuProductParameterRepository manuProductParameterRepository, IProcParameterRepository procParameterRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _localizationService = localizationService;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _procProcedureRepository = procProcedureRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procMaterialRepository = procMaterialRepository;
            _procProcessRouteRepository = procProcessRouteRepository;
            _procBomRepository = procBomRepository;
            _manuSfcRepository = manuSfcRepository;
            _procResourceRepository = procResourceRepository;
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
            _manuDowngradingRecordRepository = manuDowngradingRecordRepository;
            _manuProductBadRecordRepository = manuProductBadRecordRepository;
            _manuBarCodeRelationRepository = manuBarCodeRelationRepository;
            _manuFacePlateRepairRepository = manuFacePlateRepairRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuProductParameterRepository = manuProductParameterRepository;
            _procParameterRepository = procParameterRepository;
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
                    outStep = outSfcSteps.FirstOrDefault(x => currentStep.CreatedOn < x.CreatedOn && x.CreatedOn < nextStep.CreatedOn);
                }
                else
                {
                    // 查找出站时间
                    outStep = outSfcSteps.FirstOrDefault(x => currentStep.CreatedOn < x.CreatedOn);
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

        /// <summary>
        ///获取步骤详情
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<StepDetailDto> GetStepDetailAsync(StepQueryDto query)
        {
            //for (int index = 0; index < 2048; index++)
            //{
            //    await _manuProductParameterRepository.PrepareProductParameterSFCTable(index);
            //}
            //var procProcedureEntities1 = await _procProcedureRepository.GetEntitiesAsync(new ProcProcedureQuery { });

            //foreach (var item in procProcedureEntities1)
            //{
            //    await _manuProductParameterRepository.PrepareProductParameterProcedureIdTable(item.SiteId, item.Id);
            //}

            var stepDetailDto = new StepDetailDto();
            var manuSfcStepEntities = await _manuSfcStepRepository.GetStepsBySFCAsync(new EntityBySFCQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                SFC = query.SFC
            });

            var beforeStepEntity = manuSfcStepEntities.FirstOrDefault(x => x.Id == query.SFCStepId);
            if (beforeStepEntity == null)
            {
                return stepDetailDto;
            }
            stepDetailDto.BeforeStepDDto = new StepDto
            {
                Id = beforeStepEntity.Id,
                SiteId = beforeStepEntity.SiteId,
                SFC = beforeStepEntity.SFC,
                ProductId = beforeStepEntity.ProductId,
                WorkOrderId = beforeStepEntity.WorkOrderId,
                ProductBOMId = beforeStepEntity.ProductBOMId,
                ProcessRouteId = beforeStepEntity.ProcessRouteId,
                WorkCenterId = beforeStepEntity.WorkCenterId,
                Qty = beforeStepEntity.Qty,
                ScrapQty = beforeStepEntity.ScrapQty,
                EquipmentId = beforeStepEntity.EquipmentId,
                ResourceId = beforeStepEntity.ResourceId,
                ProcedureId = beforeStepEntity.ProcedureId,
                Operatetype = beforeStepEntity.Operatetype,
                CurrentStatus = beforeStepEntity.CurrentStatus,
                AfterOperationStatus = beforeStepEntity.AfterOperationStatus,
                RepeatedCount = beforeStepEntity.RepeatedCount,
                OperationProcedureId = beforeStepEntity.OperationProcedureId,
                OperationResourceId = beforeStepEntity.OperationResourceId,
                OperationEquipmentId = beforeStepEntity.OperationEquipmentId,
                IsRepair = beforeStepEntity.IsRepair,
                Remark = beforeStepEntity.Remark,
                SFCInfoId = beforeStepEntity.SFCInfoId,
                VehicleCode = beforeStepEntity.VehicleCode,
                CreatedBy = beforeStepEntity.CreatedBy,
            };

            await StepAssignment(stepDetailDto.BeforeStepDDto);
            var afterStepEntity = manuSfcStepEntities.FirstOrDefault(x => x.Id > beforeStepEntity.Id);
            if (afterStepEntity == null)
            {
                var manuSfcProduceEntity = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery { Sfc = beforeStepEntity.SFC, SiteId = _currentSite.SiteId ?? 0 });
                if (manuSfcProduceEntity != null)
                {
                    stepDetailDto.AfterStepDDto = new StepDto
                    {
                        SFC = manuSfcProduceEntity.SFC,
                        ProductId = manuSfcProduceEntity.ProductId,
                        ProductBOMId = manuSfcProduceEntity.ProductBOMId,
                        ProcessRouteId = manuSfcProduceEntity.ProcessRouteId,
                        WorkOrderId = manuSfcProduceEntity.WorkOrderId,
                        ProcedureId = manuSfcProduceEntity.ProcedureId,
                        ResourceId = manuSfcProduceEntity.ResourceId,
                        Qty = manuSfcProduceEntity.Qty,
                        CurrentStatus = manuSfcProduceEntity.Status
                    };
                }
                else
                {
                    var manuSfcEntity = await _manuSfcRepository.GetSingleAsync(new ManuSfcQuery
                    {
                        SiteId = _currentSite.SiteId,
                        SFC = query.SFC
                    });
                    if (manuSfcEntity == null)
                    {
                        return stepDetailDto;
                    }
                    var manuSfcInfoEntity = await _manuSfcInfoRepository.GetBySFCIdAsync(manuSfcEntity.Id);
                    stepDetailDto.AfterStepDDto = new StepDto
                    {
                        SFC = manuSfcEntity.SFC,
                        ProductId = manuSfcInfoEntity.ProductId,
                        ProductBOMId = manuSfcInfoEntity.ProductBOMId,
                        ProcessRouteId = manuSfcInfoEntity.ProcessRouteId,
                        WorkOrderId = manuSfcInfoEntity.WorkOrderId ?? 0,
                        Qty = manuSfcEntity.Qty,
                        CurrentStatus = manuSfcEntity.Status
                    };
                }
            }
            else
            {
                stepDetailDto.AfterStepDDto = new StepDto
                {
                    Id = afterStepEntity.Id,
                    SiteId = afterStepEntity.SiteId,
                    SFC = afterStepEntity.SFC,
                    ProductId = afterStepEntity.ProductId,
                    WorkOrderId = afterStepEntity.WorkOrderId,
                    ProductBOMId = afterStepEntity.ProductBOMId,
                    ProcessRouteId = afterStepEntity.ProcessRouteId,
                    WorkCenterId = afterStepEntity.WorkCenterId,
                    Qty = afterStepEntity.Qty,
                    ScrapQty = afterStepEntity.ScrapQty,
                    EquipmentId = afterStepEntity.EquipmentId,
                    ResourceId = afterStepEntity.ResourceId,
                    ProcedureId = afterStepEntity.ProcedureId,
                    Operatetype = afterStepEntity.Operatetype,
                    CurrentStatus = afterStepEntity.CurrentStatus,
                    AfterOperationStatus = afterStepEntity.AfterOperationStatus,
                    RepeatedCount = afterStepEntity.RepeatedCount,
                    OperationProcedureId = afterStepEntity.OperationProcedureId,
                    OperationResourceId = afterStepEntity.OperationResourceId,
                    OperationEquipmentId = afterStepEntity.OperationEquipmentId,
                    IsRepair = afterStepEntity.IsRepair,
                    Remark = afterStepEntity.Remark,
                    SFCInfoId = afterStepEntity.SFCInfoId,
                    VehicleCode = afterStepEntity.VehicleCode,
                };
            }
            await StepAssignment(stepDetailDto.AfterStepDDto);
            stepDetailDto.AfterStepDDto.CreatedBy = beforeStepEntity.CreatedBy;
            stepDetailDto.AfterStepDDto.CreatedOn = beforeStepEntity.CreatedOn;
           var manuSfcStepTypeobOrAssemblys = GetManuSfcStepTypeobOrAssemblys();
            var manuSfcStepTypeobOrAssembly = manuSfcStepTypeobOrAssemblys.FirstOrDefault(x => x.Key == beforeStepEntity.Operatetype);
            if (manuSfcStepTypeobOrAssembly != null)
            {
                stepDetailDto.AfterStepDDto.JobOrAssemblyCode = manuSfcStepTypeobOrAssembly.JobOrAssemblyCode;
                stepDetailDto.AfterStepDDto.JobOrAssemblyName = manuSfcStepTypeobOrAssembly.JobOrAssemblyName;
            }
            switch (beforeStepEntity.Operatetype)
            {
                case ManuSfcStepTypeEnum.Create:
                    stepDetailDto.BeforeStepDDto = new StepDto { };
                    break;
                case ManuSfcStepTypeEnum.BadEntry:
                    var manuProductBadRecordEntities = await _manuProductBadRecordRepository.GetManuProductBadRecordEntitiesBySFCAsync(new ManuProductBadRecordBySfcQuery
                    {
                        SiteId = _currentSite.SiteId ?? 0,
                        SfcStepId = beforeStepEntity.Id
                    });
                    if (manuProductBadRecordEntities != null && manuProductBadRecordEntities.Any())
                    {
                        var qualUnqualifiedCodeEntities = await _qualUnqualifiedCodeRepository.GetByIdsAsync(manuProductBadRecordEntities.Select(x => x.UnqualifiedId));
                        stepDetailDto.ExtendedProperties = qualUnqualifiedCodeEntities.Select(x => new ExtendedPropertieDto
                        {
                            Field = x.UnqualifiedCode
                        });
                    }
                    break;
                case ManuSfcStepTypeEnum.BadRejudgment:
                    var reJudgmentManuProductBadRecordEntities = await _manuProductBadRecordRepository.GetManuProductBadRecordEntitiesBySFCAsync(new ManuProductBadRecordBySfcQuery
                    {
                        SiteId = _currentSite.SiteId ?? 0,
                        ReJudgmentSfcStepId = beforeStepEntity.Id
                    });
                    if (reJudgmentManuProductBadRecordEntities != null && reJudgmentManuProductBadRecordEntities.Any())
                    {
                        var qualUnqualifiedCodeEntities = await _qualUnqualifiedCodeRepository.GetByIdsAsync(reJudgmentManuProductBadRecordEntities.Select(x => x.UnqualifiedId));

                        stepDetailDto.ExtendedProperties = reJudgmentManuProductBadRecordEntities.Select(x => new ExtendedPropertieDto
                        {
                            Field = qualUnqualifiedCodeEntities?.FirstOrDefault(o => o.Id == x.UnqualifiedId)?.UnqualifiedCode ?? "",
                            FieldValue = _localizationService.GetResource("Hymson.MES.Core.Enums.Manufacture.ProductBadDisposalResultEnum." + x.ReJudgmentResult)
                        }
                        );
                    }
                    break;
                case ManuSfcStepTypeEnum.RepairComplete:
                    var manuFacePlateRepairEntities = await _manuFacePlateRepairRepository.GetManuSfcRepairRecordEntitiesAsync(new Data.Repositories.Manufacture.ManuFacePlateRepair.Query.ManuSfcRepairRecordQuery
                    {
                        SfcStepId = beforeStepEntity.Id,
                        SiteId = _currentSite.SiteId ?? 0,
                    });
                    if (manuFacePlateRepairEntities != null && manuFacePlateRepairEntities.Any())
                    {
                        var manuSfcRepairDetailEntities = await _manuFacePlateRepairRepository.GetManuSfcRepairDetailEntitiesAsync(new Data.Repositories.Manufacture.ManuFacePlateRepair.Query.ManuSfcRepairDetailQuery
                        {
                            SiteId = _currentSite.SiteId ?? 0,
                            SfcRepairIds = manuFacePlateRepairEntities.Select(x => x.Id)
                        });
                        if (manuFacePlateRepairEntities != null && manuFacePlateRepairEntities.Any())
                        {
                            var productBadRecordEntities = await _manuProductBadRecordRepository.GetByIdsAsync(manuSfcRepairDetailEntities.Select(x => x.ProductBadId));

                            var qualUnqualifiedCodeEntities = await _qualUnqualifiedCodeRepository.GetByIdsAsync(productBadRecordEntities.Select(x => x.UnqualifiedId));
                            foreach (var item in manuSfcRepairDetailEntities)
                            {
                                var itemProductBadRecordEntities = productBadRecordEntities.Where(x => x.Id == item.ProductBadId);
                                var itemqualUnqualifiedCodeEntities = qualUnqualifiedCodeEntities.Where(x => itemProductBadRecordEntities.Select(o => o.UnqualifiedId).Contains(x.Id));
                                if (itemqualUnqualifiedCodeEntities != null && itemqualUnqualifiedCodeEntities.Any())
                                {
                                    stepDetailDto.ExtendedProperties = itemqualUnqualifiedCodeEntities.Select(x => new ExtendedPropertieDto
                                    {
                                        Field = x.UnqualifiedCode,
                                        FieldValue = _localizationService.GetResource("Hymson.MES.Core.Enums.Manufacture.ManuSfcRepairDetailIsIsCloseEnum." + item.IsClose)
                                    });
                                }
                            }
                        }
                    }

                    break;
                case ManuSfcStepTypeEnum.EnterDowngrading:
                case ManuSfcStepTypeEnum.RemoveDowngrading:
                    var manuDowngradingRecordEntities = await _manuDowngradingRecordRepository.GetManuDowngradingRecordEntitiesAsync(new ManuDowngradingRecordQuery
                    {
                        SFCStepId = beforeStepEntity.Id
                    });
                    if (manuDowngradingRecordEntities != null && manuDowngradingRecordEntities.Any())
                    {
                        stepDetailDto.ExtendedProperties = manuDowngradingRecordEntities.Select(x => new ExtendedPropertieDto
                        {
                            Field = x.Grade,
                        });
                    }
                    break;
                case ManuSfcStepTypeEnum.Marking:
                    var markingManuProductBadRecordEntities = await _manuProductBadRecordRepository.GetManuProductBadRecordEntitiesBySFCAsync(new ManuProductBadRecordBySfcQuery
                    {
                        SiteId = _currentSite.SiteId ?? 0,
                        SfcStepId = beforeStepEntity.Id
                    });
                    if (markingManuProductBadRecordEntities != null && markingManuProductBadRecordEntities.Any())
                    {
                        var procProcedureEntities = await _procProcedureRepository.GetByIdsAsync(markingManuProductBadRecordEntities.Select(x => x.InterceptOperationId ?? 0).Distinct());
                        var qualUnqualifiedCodeEntities = await _qualUnqualifiedCodeRepository.GetByIdsAsync(markingManuProductBadRecordEntities.Select(x => x.UnqualifiedId));
                        if (qualUnqualifiedCodeEntities != null && qualUnqualifiedCodeEntities.Any())
                        {
                            stepDetailDto.ExtendedProperties = markingManuProductBadRecordEntities.Select(x => new ExtendedPropertieDto
                            {
                                Field = qualUnqualifiedCodeEntities?.FirstOrDefault(o => o.Id == x.UnqualifiedId)?.UnqualifiedCode ?? "",
                                FieldValue = procProcedureEntities.FirstOrDefault(o => x.InterceptOperationId == o.Id)?.Code
                            });
                        }
                    }
                    break;
                case ManuSfcStepTypeEnum.CloseMarking:
                    var closeMarkingManuProductBadRecordEntities = await _manuProductBadRecordRepository.GetManuProductBadRecordEntitiesBySFCAsync(new ManuProductBadRecordBySfcQuery
                    {
                        SiteId = _currentSite.SiteId ?? 0,
                        CloseSfcStepId = beforeStepEntity.Id
                    });
                    if (closeMarkingManuProductBadRecordEntities != null && closeMarkingManuProductBadRecordEntities.Any())
                    {
                        var procProcedureEntities = await _procProcedureRepository.GetByIdsAsync(closeMarkingManuProductBadRecordEntities.Select(x => x.InterceptOperationId ?? 0).Distinct());
                        var qualUnqualifiedCodeEntities = await _qualUnqualifiedCodeRepository.GetByIdsAsync(closeMarkingManuProductBadRecordEntities.Select(x => x.UnqualifiedId));
                        if (qualUnqualifiedCodeEntities != null && qualUnqualifiedCodeEntities.Any())
                        {
                            stepDetailDto.ExtendedProperties = closeMarkingManuProductBadRecordEntities.Select(x => new ExtendedPropertieDto
                            {
                                Field = qualUnqualifiedCodeEntities?.FirstOrDefault(o => o.Id == x.UnqualifiedId)?.UnqualifiedCode ?? "",
                                FieldValue = procProcedureEntities.FirstOrDefault(o => x.InterceptOperationId == o.Id)?.Code
                            });
                        }
                    }
                    break;
                //case ManuSfcStepTypeEnum.InstantLock:
                //case ManuSfcStepTypeEnum.FutureLock:
                case ManuSfcStepTypeEnum.Add:
                case ManuSfcStepTypeEnum.Split:
                case ManuSfcStepTypeEnum.SfcMerge:
                    var manuBarCodeRelationEnties = await _manuBarCodeRelationRepository.GetEntitiesAsync(new ManuBarcodeRelationQuery
                    {
                        InputSfcStepId = beforeStepEntity.Id,
                        SiteId = _currentSite.SiteId ?? 0,
                    });
                    stepDetailDto.SequenceCodeHistories = manuBarCodeRelationEnties.Select(x => new SFCRelationDto
                    {
                        InputBarCode = x.InputBarCode,
                        OutputBarCode = x.OutputBarCode
                    });
                    break;
                case ManuSfcStepTypeEnum.Disassembly:
                    var disassemblyManuBarCodeRelationEnties = await _manuBarCodeRelationRepository.GetEntitiesAsync(new ManuBarcodeRelationQuery
                    {
                        DisassembledSfcStepId = beforeStepEntity.Id,
                        SiteId = _currentSite.SiteId ?? 0,
                    });
                    stepDetailDto.SequenceCodeHistories = disassemblyManuBarCodeRelationEnties.Select(x => new SFCRelationDto
                    {
                        InputBarCode = x.InputBarCode,
                        OutputBarCode = x.OutputBarCode,
                    });
                    break;

                case ManuSfcStepTypeEnum.SplitCreate:
                case ManuSfcStepTypeEnum.SfcMergeAdd:
                    var SfcMergeManuBarCodeRelationEnties = await _manuBarCodeRelationRepository.GetEntitiesAsync(new ManuBarcodeRelationQuery
                    {
                        OutputSfcStepId = beforeStepEntity.Id,
                        SiteId = _currentSite.SiteId ?? 0,
                    });
                    stepDetailDto.SequenceCodeHistories = SfcMergeManuBarCodeRelationEnties.Select(x => new SFCRelationDto
                    {
                        InputBarCode = x.InputBarCode,
                        OutputBarCode = x.OutputBarCode,
                    });
                    break;
                case ManuSfcStepTypeEnum.ParameterCollect:
                    var productParameterEntities = await _manuProductParameterRepository.GetProductParameterBySFCEntitiesAsync(new ManuProductParameterBySfcQuery
                    {
                        SFCs = new List<string> { query.SFC },
                        SiteId = _currentSite.SiteId ?? 0,
                    });
                    var sfcstepProductParameterEntities = productParameterEntities.Where(x => x.SfcstepId == beforeStepEntity.Id);
                    if (sfcstepProductParameterEntities != null && sfcstepProductParameterEntities.Any())
                    {
                        var parameterEntities = await _procParameterRepository.GetByIdsAsync(sfcstepProductParameterEntities.Select(x => x.ParameterId));

                        stepDetailDto.ExtendedProperties = sfcstepProductParameterEntities.Select(x => new ExtendedPropertieDto
                        {
                            Field = parameterEntities.FirstOrDefault(o => o.Id == x.ParameterId)?.ParameterCode ?? "",
                            FieldValue = x.ParameterValue
                        });
                    }
                    break;
            }
            return stepDetailDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stepDto"></param>
        /// <returns></returns>
        private async Task StepAssignment(StepDto stepDto)
        {
            var procMaterialEntityTask = _procMaterialRepository.GetByIdAsync(stepDto.ProductId);
            var procBomEntityTask = _procBomRepository.GetByIdAsync(stepDto.ProductBOMId ?? 0);
            var procProcessRouteEntityTask = _procProcessRouteRepository.GetByIdAsync(stepDto.ProcessRouteId ?? 0);
            var procProcedureEntityTask = _procProcedureRepository.GetByIdAsync(stepDto.ProcedureId ?? 0);
            var procResourceEntityTask = _procResourceRepository.GetByIdAsync(stepDto.ResourceId ?? 0);
            var planWorkOrderEntityTask = _planWorkOrderRepository.GetByIdAsync(stepDto.WorkOrderId);
            var procMaterialEntity = await procMaterialEntityTask;
            var procBomEntity = await procBomEntityTask;
            var procProcessRouteEntity = await procProcessRouteEntityTask;
            var procProcedureEntity = await procProcedureEntityTask;
            var procResourceEntity = await procResourceEntityTask;
            var planWorkOrderEntity = await planWorkOrderEntityTask;

            stepDto.MaterialVersion = procMaterialEntity.Version ?? "";
            stepDto.MaterialCode = procMaterialEntity.MaterialCode;
            stepDto.MaterialName = procMaterialEntity.MaterialName;
            stepDto.BomCode = procBomEntity?.BomCode ?? "";
            stepDto.BomName = procBomEntity?.BomName ?? "";
            stepDto.BomVersion = procBomEntity?.Version ?? "";
            stepDto.ProcessRouteCode = procProcessRouteEntity?.Code ?? "";
            stepDto.ProcessRouteName = procProcessRouteEntity?.Name ?? "";
            stepDto.ProcessRouteVersion = procProcessRouteEntity?.Version ?? "";
            stepDto.ProcedureCode = procProcedureEntity?.Code ?? "";
            stepDto.ProcedureName = procProcedureEntity?.Name ?? "";
            stepDto.ResourceCode = procResourceEntity?.ResCode ?? "";
            stepDto.ResourceName = procResourceEntity?.ResName ?? "";
            stepDto.OrderCode = planWorkOrderEntity?.OrderCode ?? "";
        }

        /// <summary>
        /// 使用切面来获取
        /// </summary>
        /// <returns></returns>
        private IEnumerable<GetManuSfcStepTypeJobOrAssemblyNameDto> GetManuSfcStepTypeobOrAssemblys()
        {
            var list = new List<GetManuSfcStepTypeJobOrAssemblyNameDto>();

            // 获取枚举类型
            Type enumType = typeof(ManuSfcStepTypeEnum);

            // 遍历枚举值
            foreach (var enumValue in Enum.GetValues(enumType))
            {
                if (enumValue == null) continue;
                // 获取枚举字段
                var field = enumType.GetField(enumValue.ToString());

                if (field == null) continue;

                // 获取枚举值上的 ManuSfcStepOperationTypeAttrribute 特性
                var manuSfcStepOperationTypeAttributes = field.GetCustomAttributes(typeof(ManuSfcStepOperationTypeAttrribute), false);
                var manuSfcStepOperationTypeAttribute = manuSfcStepOperationTypeAttributes.Length > 0 ? ((ManuSfcStepOperationTypeAttrribute)manuSfcStepOperationTypeAttributes[0]) : null;
                if (manuSfcStepOperationTypeAttribute != null)
                {
                    list.Add(new GetManuSfcStepTypeJobOrAssemblyNameDto
                    {
                        Key = (ManuSfcStepTypeEnum)enumValue,
                        JobOrAssemblyCode = manuSfcStepOperationTypeAttribute.JobOrAssemblyCode,
                        JobOrAssemblyName = manuSfcStepOperationTypeAttribute.JobOrAssemblyName
                    });
                }
            }

            return list;
        }
    }
}
