using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Integrated.InteCodeRule.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.Snowflake;
using Hymson.Utils;
using Microsoft.Extensions.Logging;

namespace Hymson.MES.CoreServices.Services.Job
{
    [Job("条码生成", JobTypeEnum.Standard)]
    public class GenerateBarcodeJobSerice : IJobService
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly ILogger<BarcodeReceiveService> _logger;

        /// <summary>
        /// 条码接收 仓储
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IManuSfcRepository _manuSfcRepository;

        private readonly IProcMaterialRepository _procMaterialRepository;
        /// <summary>
        /// 仓储接口（资源维护）
        /// </summary>
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly IMasterDataService _masterDataService;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;
        private readonly IPlanWorkOrderBindRepository _planWorkOrderBindRepository;
        private readonly IManuCommonService _manuCommonService;
        private readonly IManuSfcStepRepository _manuSfcStepRepository;
        private readonly IPlanWorkOrderActivationRepository _planWorkOrderActivationRepository;
        private readonly IInteCodeRulesRepository _inteCodeRulesRepository;
        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteDetailNodeRepository;
        private readonly IProcProcessRouteRepository _procProcessRouteRepository;
        private readonly IInteCodeRulesMakeRepository _inteCodeRulesMakeRepository;
        private readonly IManuGenerateBarcodeService _manuGenerateBarcodeService;
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="masterDataService"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="planWorkOrderBindRepository"></param>
        /// <param name="manuCommonService"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="planWorkOrderActivationRepository"></param>
        /// <param name="inteCodeRulesRepository"></param>
        /// <param name="procProcessRouteDetailNodeRepository"></param>
        /// <param name="procProcessRouteRepository"></param>
        /// <param name="inteCodeRulesMakeRepository"></param>
        /// <param name="manuGenerateBarcodeService"></param>
        /// <param name="inteWorkCenterRepository"></param>
        public GenerateBarcodeJobSerice(ILogger<BarcodeReceiveService> logger, IPlanWorkOrderRepository planWorkOrderRepository, IManuSfcRepository manuSfcRepository, IProcMaterialRepository procMaterialRepository, IProcResourceRepository procResourceRepository, IMasterDataService masterDataService, IManuSfcInfoRepository manuSfcInfoRepository, IManuSfcProduceRepository manuSfcProduceRepository, IPlanWorkOrderBindRepository planWorkOrderBindRepository, IManuCommonService manuCommonService, IManuSfcStepRepository manuSfcStepRepository, IPlanWorkOrderActivationRepository planWorkOrderActivationRepository, IInteCodeRulesRepository inteCodeRulesRepository, IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository, IProcProcessRouteRepository procProcessRouteRepository, IInteCodeRulesMakeRepository inteCodeRulesMakeRepository, IManuGenerateBarcodeService manuGenerateBarcodeService, IInteWorkCenterRepository inteWorkCenterRepository)
        {
            _logger = logger;
            _planWorkOrderRepository = planWorkOrderRepository;
            _manuSfcRepository = manuSfcRepository;
            _procMaterialRepository = procMaterialRepository;
            _procResourceRepository = procResourceRepository;
            _masterDataService = masterDataService;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _planWorkOrderBindRepository = planWorkOrderBindRepository;
            _manuCommonService = manuCommonService;
            _manuSfcStepRepository = manuSfcStepRepository;
            _planWorkOrderActivationRepository = planWorkOrderActivationRepository;
            _inteCodeRulesRepository = inteCodeRulesRepository;
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
            _procProcessRouteRepository = procProcessRouteRepository;
            _inteCodeRulesMakeRepository = inteCodeRulesMakeRepository;
            _manuGenerateBarcodeService = manuGenerateBarcodeService;
            _inteWorkCenterRepository = inteWorkCenterRepository;
        }

        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;
        }
        public async Task<IEnumerable<JobBo>?> AfterExecuteAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;
            return null;
        }

        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            if (param is not JobRequestBo commonBo) return default;
            if (commonBo == null) return default;
            if (commonBo.SFCs != null && commonBo.SFCs.Any()) return default;
            var resourceEntity = await _procResourceRepository.GetByIdAsync(commonBo.ResourceId)
             ?? throw new CustomerValidationException(nameof(ErrorCode.MES16337));

            // 获取绑定工单
            var planWorkOrderBindEntity = await _planWorkOrderBindRepository.GetByResourceIDAsync(new PlanWorkOrderBindByResourceIdQuery
            {
                SiteId = commonBo.SiteId,
                ResourceId = commonBo.ResourceId
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES19928)).WithData("ResCode", resourceEntity.ResCode);

            var planWorkOrderEntity = await _masterDataService.GetProduceWorkOrderByIdAsync(new WorkOrderIdBo
            {
                WorkOrderId = planWorkOrderBindEntity.WorkOrderId,
                IsVerifyActivation = false
            });

            var planWorkOrderActivationEntity = await _planWorkOrderActivationRepository.GetByWorkOrderIdAsync(planWorkOrderBindEntity.WorkOrderId);
            if (planWorkOrderActivationEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19937)).WithData("WorkOrderCode", planWorkOrderEntity.OrderCode);
            }

            var inteWorkCenterEntity = await _inteWorkCenterRepository.GetByResourceIdAsync(commonBo.ResourceId);
            if (inteWorkCenterEntity == null)
            {
                var procResourceEntity = await _procResourceRepository.GetByIdAsync(commonBo.ResourceId);
                throw new CustomerValidationException(nameof(ErrorCode.MES16511)).WithData("Code", procResourceEntity.ResCode);
            }

            // 获取产出设置的产品ID
            var productIdOfSet = await _masterDataService.GetProductSetIdAsync(new ProductSetBo
            {
                SiteId = commonBo.SiteId,
                ProductId = planWorkOrderEntity.ProductId,
                ProcedureId = commonBo.ProcedureId,
                ResourceId = commonBo.ResourceId
            });

            // 产品ID
            var productId = productIdOfSet ?? planWorkOrderEntity.ProductId;

            var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(productId);
            var inteCodeRulesEntity = await _inteCodeRulesRepository.GetInteCodeRulesByProductIdAsync(new InteCodeRulesByProductQuery
            {
                ProductId = productId,
                CodeType = CodeRuleCodeTypeEnum.ProcessControlSeqCode
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES16501)).WithData("product", procMaterialEntity.MaterialCode);

            var BatchQty = string.IsNullOrEmpty(procMaterialEntity.Batch) ? 0 : decimal.Parse(procMaterialEntity.Batch);
            if (BatchQty == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16502)).WithData("product", procMaterialEntity.MaterialCode);
            }
            var qty = string.IsNullOrEmpty(procMaterialEntity.Batch) ? 0 : decimal.Parse(procMaterialEntity.Batch);

            var processRouteDetailNodeEntities = await _procProcessRouteDetailNodeRepository.GetProcessRouteDetailNodesByProcessRouteIdAsync(planWorkOrderEntity.ProcessRouteId);
            var processRouteDetailNodeEntity = processRouteDetailNodeEntities.FirstOrDefault(x => x.ProcedureId == commonBo.ProcedureId);
            if (processRouteDetailNodeEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16509));
            }

            var processRouteEntity = await _procProcessRouteRepository.GetByIdAsync(processRouteDetailNodeEntity.ProcessRouteId);

            // 读取基础数据
            var codeRulesMakeList = await _inteCodeRulesMakeRepository.GetInteCodeRulesMakeEntitiesAsync(new InteCodeRulesMakeQuery
            {
                SiteId = param.SiteId,
                CodeRulesId = inteCodeRulesEntity.Id
            });

            if (codeRulesMakeList == null || !codeRulesMakeList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16501)).WithData("product", procMaterialEntity.MaterialCode);
            }

            var barcodeList = await _manuGenerateBarcodeService.GenerateBarCodeSerialNumberReturnBarCodeInfosAsync(new BarCodeSerialNumberBo
            {
                IsTest = false,
                IsSimulation = false,
                CodeRulesMakeBos = codeRulesMakeList.Select(s => new CodeRulesMakeBo
                {
                    Seq = s.Seq,
                    ValueTakingType = s.ValueTakingType,
                    SegmentedValue = s.SegmentedValue,
                    CustomValue = s.CustomValue,
                }),
                ProductId = procMaterialEntity.Id,
                CodeRuleKey = $"{inteCodeRulesEntity.Id}",
                Count = 1,
                Base = inteCodeRulesEntity.Base,
                Increment = inteCodeRulesEntity.Increment,
                IgnoreChar = inteCodeRulesEntity.IgnoreChar ?? "",
                OrderLength = inteCodeRulesEntity.OrderLength,
                ResetType = inteCodeRulesEntity.ResetType,
                StartNumber = inteCodeRulesEntity.StartNumber,
                CodeMode = inteCodeRulesEntity.CodeMode,
                SiteId = param.SiteId,
                InteWorkCenterId = inteWorkCenterEntity.Id
            });

            List<ManuSfcEntity> manuSfcList = new List<ManuSfcEntity>();
            List<ManuSfcEntity> updateManuSfcList = new List<ManuSfcEntity>();
            List<ManuSfcInfoEntity> manuSfcInfoList = new List<ManuSfcInfoEntity>();
            List<ManuSfcProduceEntity> manuSfcProduceList = new List<ManuSfcProduceEntity>();
            List<ManuSfcStepEntity> manuSfcStepList = new List<ManuSfcStepEntity>();
            var sfcs = new List<string>();
            foreach (var item in barcodeList)
            {
                foreach (var sfc in item.BarCodes)
                {
                    sfcs.Add(sfc);
                    var manuSfcEntity = new ManuSfcEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = commonBo.SiteId,
                        SFC = sfc,
                        Qty = qty,
                        IsUsed = YesOrNoEnum.No,
                        Status = SfcStatusEnum.lineUp,
                        CreatedBy = commonBo.UserName,
                        UpdatedBy = commonBo.UserName
                    };
                    manuSfcList.Add(manuSfcEntity);
                    var sfcInfoId = IdGenProvider.Instance.CreateId();
                    manuSfcInfoList.Add(new ManuSfcInfoEntity
                    {
                        Id = sfcInfoId,
                        SiteId = commonBo.SiteId,
                        SfcId = manuSfcEntity.Id,
                        WorkOrderId = planWorkOrderEntity.Id,
                        ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
                        ProductBOMId = planWorkOrderEntity.ProductBOMId,
                        ProductId = productId,
                        IsUsed = true,
                        CreatedBy = commonBo.UserName,
                        CreatedOn = commonBo.Time,
                        UpdatedBy = commonBo.UserName,
                        UpdatedOn = commonBo.Time
                    });

                    manuSfcProduceList.Add(new ManuSfcProduceEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = commonBo.SiteId,
                        SFC = sfc,
                        SFCId = manuSfcEntity.Id,
                        ProductId = productId,
                        WorkOrderId = planWorkOrderEntity.Id,
                        BarCodeInfoId = sfcInfoId,
                        ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
                        WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                        ProductBOMId = planWorkOrderEntity.ProductBOMId,
                        Qty = qty,
                        ResourceId = commonBo.ResourceId,
                        EquipmentId = commonBo.EquipmentId,
                        ProcedureId = commonBo.ProcedureId,
                        Status = SfcStatusEnum.lineUp,
                        RepeatedCount = 0,
                        IsScrap = TrueOrFalseEnum.No,
                        CreatedBy = commonBo.UserName,
                        CreatedOn = commonBo.Time,
                        UpdatedBy = commonBo.UserName,
                        UpdatedOn = commonBo.Time
                    });

                    manuSfcStepList.Add(new ManuSfcStepEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = commonBo.SiteId,
                        SFC = sfc,
                        ProductId = productId,
                        WorkOrderId = planWorkOrderEntity.Id,
                        WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                        Qty = qty,
                        Operatetype = ManuSfcStepTypeEnum.Receive,
                        CurrentStatus = SfcStatusEnum.lineUp,
                        OperationProcedureId= commonBo.ProcedureId,
                        OperationResourceId = commonBo.ResourceId,
                        CreatedBy = commonBo.UserName,
                        CreatedOn = commonBo.Time,
                        UpdatedBy = commonBo.UserName,
                        UpdatedOn = commonBo.Time
                    });
                }
            }
            commonBo.SFCs = sfcs;

            return new GenerateBarcodeJobResponseBo
            {
                WorkOrderId = planWorkOrderEntity.Id,
                PlanQuantity = planWorkOrderEntity.Qty * (1 + planWorkOrderEntity.OverScale / 100),
                PassDownQuantity = manuSfcProduceList.Sum(x => x.Qty),
                UserName = commonBo.UserName,
                IsProductSame = productId == planWorkOrderEntity.ProductId,
                ManuSfcList = manuSfcList,
                ManuSfcInfoList = manuSfcInfoList,
                ManuSfcProduceList = manuSfcProduceList,
                ManuSfcStepList = manuSfcStepList,
            };
        }

        public async Task<JobResponseBo> ExecuteAsync(object obj)
        {
            JobResponseBo responseBo = new();
            if (obj is not GenerateBarcodeJobResponseBo data) return responseBo;

            // 当产出设置的产品和工单对应的产品一致时，才更新工单的下达数量
            if (data.IsProductSame)
            {
                responseBo.Rows += await _planWorkOrderRepository.UpdatePassDownQuantityByWorkOrderIdAsync(new UpdatePassDownQuantityCommand
                {
                    WorkOrderId = data.WorkOrderId,
                    PlanQuantity = data.PlanQuantity,
                    PassDownQuantity = data.PassDownQuantity,
                    UserName = data.UserName,
                    UpdateDate = HymsonClock.Now()
                });

                if (responseBo.Rows == 0) throw new CustomerValidationException(nameof(ErrorCode.MES16503)).WithData("workorder", data.OrderCode);
            }

            // 更新数据
            List<Task<int>> tasks = new()
            {
                _manuSfcRepository.InsertRangeAsync(data.ManuSfcList),
                _manuSfcInfoRepository.InsertsAsync(data.ManuSfcInfoList),
                _manuSfcProduceRepository.InsertRangeAsync(data.ManuSfcProduceList),
                _manuSfcStepRepository.InsertRangeAsync(data.ManuSfcStepList),
            };

            // 等待所有任务完成
            var rowArray = await Task.WhenAll(tasks);
            responseBo.Rows += rowArray.Sum();

            return responseBo;
        }

        public async Task<IEnumerable<JobBo>?> BeforeExecuteAsync<T>(T param) where T : JobBaseBo
        {
            return new List<JobBo>();
        }

    }
}
