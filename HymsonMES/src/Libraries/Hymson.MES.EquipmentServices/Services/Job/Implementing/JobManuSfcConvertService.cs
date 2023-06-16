using Hymson.Infrastructure.Exceptions;
using Hymson.Kafka.Debezium;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.MaskCode;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.MES.EquipmentServices.Bos.Manufacture;
using Hymson.MES.EquipmentServices.Services.Common;
using Hymson.MES.EquipmentServices.Services.Manufacture.InStation;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.Web.Framework.WorkContext;

namespace Hymson.MES.EquipmentServices.Services.Job.Implementing
{
    /// <summary>
    /// SFC转换
    /// </summary>
    public class JobManuSfcConvertService : IJobManufactureService
    {
        private readonly ICurrentEquipment _currentEquipment;
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;
        private readonly IManuSfcStepRepository _manuSfcStepRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IProcResourceEquipmentBindRepository _procResourceEquipmentBindRepository;

        /// <summary>
        /// 仓储接口（掩码规则维护）
        /// </summary>
        private readonly IProcMaskCodeRuleRepository _procMaskCodeRuleRepository;

        /// <summary>
        /// 仓储接口（BBOM）
        /// </summary>
        private readonly IProcBomRepository _procBomRepository;
        /// <summary>
        /// 仓储接口（BOM明细）
        /// </summary>
        private readonly IProcBomDetailRepository _procBomDetailRepository;

        /// <summary>
        /// 仓储接口（公共方法）
        /// </summary>
        private readonly ICommonService _manuCommonOldService;

        /// <summary>
        /// 工单激活（物理删除）仓储接口
        /// </summary>
        private readonly IPlanWorkOrderBindRepository _planWorkOrderBindRepository;
        /// <summary>
        ///  进站仓储接口
        /// </summary>
        private readonly IInStationService _inStationService;


        /// <summary>
        ///  仓储（物料库存）
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentEquipment"></param>
        /// <param name="procMaskCodeRuleRepository"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="inteWorkCenterRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="procResourceEquipmentBindRepository"></param>
        /// <param name="procBomRepository"></param>
        /// <param name="procBomDetailRepository"></param>
        /// <param name="manuCommonOldService"></param>
        /// <param name="planWorkOrderBindRepository"></param>
        /// <param name="inStationService"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="localizationService"></param> 
        public JobManuSfcConvertService(ICurrentEquipment currentEquipment, IProcMaskCodeRuleRepository procMaskCodeRuleRepository,
            IProcResourceRepository procResourceRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuSfcRepository manuSfcRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IInteWorkCenterRepository inteWorkCenterRepository,
            IProcProcedureRepository procProcedureRepository,
            IProcResourceEquipmentBindRepository procResourceEquipmentBindRepository,
            IProcBomRepository procBomRepository,
            IProcBomDetailRepository procBomDetailRepository, ICommonService manuCommonOldService,
            IPlanWorkOrderBindRepository planWorkOrderBindRepository, IInStationService inStationService,
            IWhMaterialInventoryRepository whMaterialInventoryRepository, ILocalizationService localizationService)
        {
            _currentEquipment = currentEquipment;
            _procMaskCodeRuleRepository = procMaskCodeRuleRepository;
            _procResourceRepository = procResourceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _procProcedureRepository = procProcedureRepository;
            _procResourceEquipmentBindRepository = procResourceEquipmentBindRepository;
            _procBomRepository = procBomRepository;
            _procBomDetailRepository = procBomDetailRepository;
            _manuCommonOldService = manuCommonOldService;
            _planWorkOrderBindRepository = planWorkOrderBindRepository;
            _inStationService = inStationService;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _localizationService = localizationService;
        }


        /// <summary>
        /// 验证参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync(Dictionary<string, string>? param)
        {
            if (param == null ||
                param.ContainsKey("SFC") == false
                || param.ContainsKey("EquipmentId") == false
                || param.ContainsKey("ResourceId") == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16312));
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// 条码转换（模组形态转换-条码=》工单）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<JobResponseDto> ExecuteAsync(Dictionary<string, string>? param)
        {
            var defaultDto = new JobResponseDto { };

            var bo = new ManufactureDto
            {
                SFC = param["SFC"],
                EquipmentId = param["EquipmentId"].ParseToLong(),
                ResourceId = param["ResourceId"].ParseToLong()
            };

            //获取资源
            var procResource = await _procResourceRepository.GetByIdAsync(bo.ResourceId);
            //查询资源和设备是否绑定
            var resourceEquipmentBindQuery = new ProcResourceEquipmentBindQuery
            {
                SiteId = _currentEquipment.SiteId,
                Ids = new long[] { _currentEquipment.Id ?? 0 },
                ResourceId = bo.ResourceId,
            };
            var resEquipentBind = await _procResourceEquipmentBindRepository.GetByResourceIdAsync(resourceEquipmentBindQuery);
            if (resEquipentBind.Any() == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19910)).WithData("ResCode", procResource.ResCode).WithData("EquCode", _currentEquipment.Code);
            }

            ////查找当前工作中心（产线）
            //var workLine = await _inteWorkCenterRepository.GetByResourceIdAsync(procResource.Id);
            //if (workLine == null)
            //{
            //    //通过资源未找到关联产线
            //    throw new CustomerValidationException(nameof(ErrorCode.MES19911)).WithData("ResourceCode", procResource.ResCode);
            //}
            ////查找激活工单  
            //var planWorkOrders = await _planWorkOrderRepository.GetByWorkLineIdAsync(workLine.Id);
            //if (planWorkOrders == null || !planWorkOrders.Any())
            //{
            //    //产线未激活工单
            //    throw new CustomerValidationException(nameof(ErrorCode.MES19912)).WithData("WorkCenterCode", workLine.Code);
            //}
            ////不考虑混线
            //var planWorkOrder = planWorkOrders.First();

            //TODO 这里应该查资源绑定的工单
            var planWorkOrderBindEntity = await _planWorkOrderBindRepository.GetByResourceIDAsync(new PlanWorkOrderBindByResourceIdQuery { SiteId = _currentEquipment.SiteId, EquipmentId = _currentEquipment.Id ?? 0, ResourceId = procResource.Id });
            if (planWorkOrderBindEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19928)).WithData("ResCode", procResource.ResCode);
            }
            //获取工单  带验证
            var planWorkOrder = await _manuCommonOldService.GetProduceWorkOrderByIdAsync(planWorkOrderBindEntity.WorkOrderId);
            if (planWorkOrder == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19929));
            }

            //获取当前资源绑定的工序
            var procedureEntity = await _procProcedureRepository.GetProcProdureByResourceIdAsync(new ProcProdureByResourceIdQuery { ResourceId = bo.ResourceId, SiteId = _currentEquipment.SiteId });
            if (procedureEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19913)).WithData("ResCode", procResource.ResCode);
            }
            //获取当前物料
            var bomEntity = await _procBomRepository.GetByIdAsync(planWorkOrder.ProductBOMId);
            if (bomEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19914)).WithData("OrderCode", planWorkOrder.OrderCode);
            }
            //获取BOM对应当前工序的物料
            var bomDetailList = await _procBomDetailRepository.GetByBomIdAndProcedureIdAsync(new ProcBomDetailByBomIdAndProcedureIdQuery { BomId = bomEntity.Id, ProcedureId = procedureEntity.Id });
            if (bomDetailList == null || !bomDetailList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19915)).WithData("BomCode", bomEntity.BomCode);
            }
            var bomDetail = bomDetailList.FirstOrDefault();
            //验证掩码规则
            var isCodeRule = await _manuCommonOldService.CheckBarCodeByMaskCodeRuleAsync(bo.SFC, bomDetail.MaterialId);
            if (!isCodeRule)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19916)).WithData("SFC", bo.SFC);
            }
            //获取SFC
            var sfcEntity = await _manuSfcRepository.GetBySFCAsync(new GetBySFCQuery { SFC = bo.SFC, SiteId = _currentEquipment.SiteId });
            //if (sfcEntity != null)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES19917)).WithData("SFC", bo.SFC);
            //}
            var processRouteFirstProcedure = await _manuCommonOldService.GetFirstProcedureAsync(planWorkOrder.ProcessRouteId);
            if (sfcEntity != null)
            {
                switch (sfcEntity.Status)
                {
                    case SfcStatusEnum.InProcess:
                        //获取在制进站
                        var manuSfcProduceEntity = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery { Sfc = bo.SFC, SiteId = _currentEquipment.SiteId });
                        if (manuSfcProduceEntity == null)
                        {
                            throw new CustomerValidationException(nameof(ErrorCode.MES19930)).WithData("SFC", bo.SFC);
                        }
                        //当前工序在制
                        if (manuSfcProduceEntity.Status == SfcProduceStatusEnum.lineUp && procedureEntity.Id == manuSfcProduceEntity.ProcedureId)
                        {
                            //进站
                            await _inStationService.InStationAsync(manuSfcProduceEntity);
                        }
                        else
                        {
                            var statusMsg = _localizationService.GetResource($"Hymson.MES.Core.Enums.SfcProduceStatusEnum.{Enum.GetName(typeof(SfcProduceStatusEnum), manuSfcProduceEntity.Status) ?? ""}");
                            var procedure = await _procProcedureRepository.GetByIdAsync(manuSfcProduceEntity.ProcedureId);
                            var procedureMsg = procedure == null ? "" : procedure.Code;
                            throw new CustomerValidationException(nameof(ErrorCode.MES19933)).WithData("SFC", bo.SFC).WithData("Procedure", procedureMsg).WithData("Status", statusMsg);
                        }
                        break;
                    //完成或入库=》扣减库存
                    case SfcStatusEnum.Complete:
                    case SfcStatusEnum.Received:
                        var manuSfcInfoEntity = _manuSfcInfoRepository.GetBySFCAsync(sfcEntity.Id);
                        //库存
                        var updateQuantityCommand = new UpdateQuantityCommand
                        {
                            BarCode = bo.SFC,
                            QuantityResidue = 1,
                            UpdatedBy = _currentEquipment.Name
                        };
                        //SFC
                        var updateSfc = new ManuSfcUpdateStatusAndIsUsedCommand
                        {
                            Sfcs = new string[] { bo.SFC },
                            Status = SfcStatusEnum.InProcess,
                            IsUsed = YesOrNoEnum.Yes,
                            UserId = _currentEquipment.Name,
                            UpdatedOn = HymsonClock.Now()
                        };

                        //更新旧条码在用
                        var updateSfcInfoIsUsed = new ManuSfcInfoUpdateIsUsedCommand
                        {
                            IsUsed = false,
                            SfcIds = new long[] { sfcEntity.Id },
                            UpdatedOn = HymsonClock.Now(),
                            UserId = _currentEquipment.Name
                        };

                        //新增条码明细
                        ManuSfcInfoEntity createSfcInfo = new()
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteId = _currentEquipment.SiteId,
                            IsUsed = true,
                            ProductId = bomDetail.MaterialId,
                            SfcId = sfcEntity.Id,
                            WorkOrderId = planWorkOrder.Id,
                            CreatedBy = _currentEquipment.Name,
                            UpdatedBy = _currentEquipment.Name
                        };

                        //在制
                        var createManuSfcProduce = new ManuSfcProduceEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteId = _currentEquipment.SiteId,
                            SFC = bo.SFC,
                            ProductId = planWorkOrder.ProductId,
                            WorkOrderId = planWorkOrder.Id,
                            BarCodeInfoId = manuSfcInfoEntity.Id,
                            ProcessRouteId = planWorkOrder.ProcessRouteId,
                            WorkCenterId = planWorkOrder.WorkCenterId ?? 0,
                            ProductBOMId = planWorkOrder.ProductBOMId,
                            Qty = 1,
                            ProcedureId = processRouteFirstProcedure.ProcedureId,
                            Status = SfcProduceStatusEnum.Activity,//直接活动  不用再进站
                            RepeatedCount = 0,
                            IsScrap = TrueOrFalseEnum.No,
                            CreatedBy = _currentEquipment.Name,
                            UpdatedBy = _currentEquipment.Name
                        };
                        //步骤 进站
                        var createManuSfcStep = new ManuSfcStepEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteId = _currentEquipment.SiteId,
                            SFC = bo.SFC,
                            ProductId = planWorkOrder.ProductId,
                            WorkOrderId = planWorkOrder.Id,
                            ProductBOMId = planWorkOrder.ProductBOMId,
                            WorkCenterId = planWorkOrder.WorkCenterId ?? 0,
                            Qty = 1,
                            ProcedureId = processRouteFirstProcedure.ProcedureId,
                            Operatetype = ManuSfcStepTypeEnum.InStock,
                            CurrentStatus = SfcProduceStatusEnum.Activity,
                            CreatedBy = _currentEquipment.Name,
                            UpdatedBy = _currentEquipment.Name
                        };

                        //入库
                        using (var trans = TransactionHelper.GetTransactionScope())
                        {
                            await _whMaterialInventoryRepository.UpdateReduceQuantityResidueAsync(updateQuantityCommand);

                            await _manuSfcRepository.UpdateSfcStatusAndIsUsedAsync(updateSfc);

                            await _manuSfcInfoRepository.UpdatesIsUsedAsync(updateSfcInfoIsUsed);

                            await _manuSfcInfoRepository.InsertAsync(createSfcInfo);

                            await _manuSfcProduceRepository.InsertAsync(createManuSfcProduce);

                            await _manuSfcStepRepository.InsertAsync(createManuSfcStep);

                            trans.Complete();
                        }

                        break;
                    case SfcStatusEnum.Scrapping:
                        throw new CustomerValidationException(nameof(ErrorCode.MES19932)).WithData("SFC", bo.SFC);
                }

            }
            else
            {
                //条码
                var createmanuSfcEntity = new ManuSfcEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    SFC = bo.SFC,
                    Qty = 1,
                    IsUsed = YesOrNoEnum.Yes,
                    Status = SfcStatusEnum.InProcess,
                    CreatedBy = _currentEquipment.Name,
                    UpdatedBy = _currentEquipment.Name
                };

                //明细
                var createManuSfcInfo = new ManuSfcInfoEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    SfcId = createmanuSfcEntity.Id,
                    WorkOrderId = planWorkOrder.Id,
                    ProductId = bomDetail.MaterialId,
                    IsUsed = true,
                    CreatedBy = _currentEquipment.Name,
                    UpdatedBy = _currentEquipment.Name
                };

                //在制
                var createManuSfcProduce = new ManuSfcProduceEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    SFC = bo.SFC,
                    ProductId = planWorkOrder.ProductId,
                    WorkOrderId = planWorkOrder.Id,
                    BarCodeInfoId = createmanuSfcEntity.Id,
                    ProcessRouteId = planWorkOrder.ProcessRouteId,
                    WorkCenterId = planWorkOrder.WorkCenterId ?? 0,
                    ProductBOMId = planWorkOrder.ProductBOMId,
                    Qty = 1,
                    ProcedureId = processRouteFirstProcedure.ProcedureId,
                    Status = SfcProduceStatusEnum.Activity,//直接活动  不用再进站
                    RepeatedCount = 0,
                    IsScrap = TrueOrFalseEnum.No,
                    CreatedBy = _currentEquipment.Name,
                    UpdatedBy = _currentEquipment.Name
                };

                var createManuSfcStepList = new List<ManuSfcStepEntity>
                {
                    //步骤 转换
                    new ManuSfcStepEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentEquipment.SiteId,
                        SFC = bo.SFC,
                        ProductId = planWorkOrder.ProductId,
                        WorkOrderId = planWorkOrder.Id,
                        ProductBOMId = planWorkOrder.ProductBOMId,
                        WorkCenterId = planWorkOrder.WorkCenterId ?? 0,
                        Qty = 1,
                        ProcedureId = processRouteFirstProcedure.ProcedureId,
                        Operatetype = ManuSfcStepTypeEnum.Change,
                        CurrentStatus = SfcProduceStatusEnum.lineUp,
                        CreatedBy = _currentEquipment.Name,
                        UpdatedBy = _currentEquipment.Name
                    },
                    //步骤 进站
                    new ManuSfcStepEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentEquipment.SiteId,
                        SFC = bo.SFC,
                        ProductId = planWorkOrder.ProductId,
                        WorkOrderId = planWorkOrder.Id,
                        ProductBOMId = planWorkOrder.ProductBOMId,
                        WorkCenterId = planWorkOrder.WorkCenterId ?? 0,
                        Qty = 1,
                        ProcedureId = processRouteFirstProcedure.ProcedureId,
                        Operatetype = ManuSfcStepTypeEnum.InStock,
                        CurrentStatus = SfcProduceStatusEnum.Activity,
                        CreatedBy = _currentEquipment.Name,
                        UpdatedBy = _currentEquipment.Name
                    }
                };

                //事务入库
                using var ts = TransactionHelper.GetTransactionScope();

                await _manuSfcRepository.InsertAsync(createmanuSfcEntity);

                await _manuSfcInfoRepository.InsertAsync(createManuSfcInfo);

                await _manuSfcProduceRepository.InsertAsync(createManuSfcProduce);

                await _manuSfcStepRepository.InsertRangeAsync(createManuSfcStepList);

                ts.Complete();
            }


            return defaultDto;
        }
    }
}
