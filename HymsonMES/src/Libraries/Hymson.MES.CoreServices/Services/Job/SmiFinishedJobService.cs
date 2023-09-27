using Hymson.EventBus.Abstractions;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.Warehouse;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.CoreServices.Services.NewJob
{
    /// <summary>
    /// 半成品完成
    /// </summary>
    [Job("半成品完成", JobTypeEnum.Standard)]
    public class SmiFinishedJobService : IJobService
    {
        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;

        /// <summary>
        /// 仓储接口（生产工单）
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 仓储接口（条码信息）
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 仓储接口（条码流转）
        /// </summary>
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;

        /// <summary>
        /// 仓储接口（物料库存）
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;

        /// <summary>
        /// 仓储接口（物料台账）
        /// </summary>
        private readonly IWhMaterialStandingbookRepository _whMaterialStandingbookRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 事件总线
        /// </summary>
        private readonly IEventBus<EventBusInstance1> _eventBus;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="masterDataService"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuSfcCirculationRepository"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="whMaterialStandingbookRepository"></param>
        /// <param name="localizationService"></param>
        /// <param name="eventBus"></param>
        public SmiFinishedJobService(IMasterDataService masterDataService,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IManuSfcRepository manuSfcRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IWhMaterialStandingbookRepository whMaterialStandingbookRepository,
            ILocalizationService localizationService,
            IEventBus<EventBusInstance1> eventBus)
        {
            _masterDataService = masterDataService;
            _planWorkOrderRepository = planWorkOrderRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _whMaterialStandingbookRepository = whMaterialStandingbookRepository;
            _localizationService = localizationService;
            _eventBus = eventBus;
        }


        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<SmiFinishedRequestBo>();
            if (bo == null) return;

            // 获取生产条码信息
            var sfcProduceEntities = await bo.Proxy!.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsWithCheckAsync, bo);
            if (sfcProduceEntities == null || !sfcProduceEntities.Any()) return;

            // 判断条码锁状态
            await bo.Proxy.GetValueAsync(_masterDataService.GetProduceBusinessEntitiesBySFCsAsync, bo);

            // 合法性校验
            sfcProduceEntities.VerifySFCStatus(SfcStatusEnum.Activity, _localizationService.GetResource($"{typeof(SfcStatusEnum).FullName}.{nameof(SfcStatusEnum.Activity)}"))
                              .VerifyProcedure(bo.ProcedureId)
                              .VerifyResource(bo.ResourceId);

            //（前提：这些条码都是同一工单同一工序）
            var firstProduceEntity = sfcProduceEntities.FirstOrDefault();
            if (firstProduceEntity == null) return;

            // 验证条码对应的物料ID是否和工单物料ID一致
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(firstProduceEntity.WorkOrderId)
             ?? throw new CustomerValidationException(nameof(ErrorCode.MES16301));

            if (firstProduceEntity.ProductId == planWorkOrderEntity.ProductId)
            {
                // 结束
                throw new CustomerValidationException(nameof(ErrorCode.MES18219));
            }

            // 判断条码状态是否是"完成"
            var sfcEntities = await _manuSfcRepository.GetBySFCsAsync(bo.SFCs)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES17104));

            if (sfcEntities.Any(a => a.Status == SfcStatusEnum.Complete))
            {
                // 结束
                throw new CustomerValidationException(nameof(ErrorCode.MES18220));
            }
        }

        /// <summary>
        /// 执行前节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> BeforeExecuteAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;
            return null;
        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<SmiFinishedRequestBo>();
            if (bo == null) return default;

            // 待执行的命令
            SmiFinisheResponseBo responseBo = new();

            // 获取生产条码信息
            var sfcProduceEntities = await bo.Proxy!.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsWithCheckAsync, bo);
            if (sfcProduceEntities == null || !sfcProduceEntities.Any()) return default;

            // 条码信息
            var manuSFCEntities = await _manuSfcRepository.GetByIdsAsync(sfcProduceEntities.Select(s => s.SFCId));

            var firstSFCProduceEntity = sfcProduceEntities.FirstOrDefault();
            if (firstSFCProduceEntity == null) return default;

            // 更新时间
            var updatedBy = bo.UserName;
            var updatedOn = HymsonClock.Now();

            // 读取条码信息
            var manuSfcEntities = await _masterDataService.GetManuSFCEntitiesWithNullCheckAsync(bo);

            // 读取产品基础信息
            var procMaterialEntity = await _masterDataService.GetProcMaterialEntityWithNullCheckAsync(firstSFCProduceEntity.ProductId);

            // 组装（出站步骤数据）
            responseBo.SFCStepEntities = sfcProduceEntities.Select(sfcProduceEntity => new ManuSfcStepEntity
            {
                // 插入 manu_sfc_step 状态为出站（默认值）
                Operatetype = ManuSfcStepTypeEnum.OutStock,
                CurrentStatus = SfcStatusEnum.Complete,
                Id = IdGenProvider.Instance.CreateId(),
                SFC = sfcProduceEntity.SFC,
                ProductId = sfcProduceEntity.ProductId,
                WorkOrderId = sfcProduceEntity.WorkOrderId,
                WorkCenterId = sfcProduceEntity.WorkCenterId,
                ProductBOMId = sfcProduceEntity.ProductBOMId,
                ProcedureId = sfcProduceEntity.ProcedureId,
                Qty = sfcProduceEntity.Qty,
                EquipmentId = sfcProduceEntity.EquipmentId,
                ResourceId = sfcProduceEntity.ResourceId,
                SiteId = bo.SiteId,
                CreatedBy = updatedBy,
                CreatedOn = updatedOn,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn
            });

            // 删除 manu_sfc_produce_business
            responseBo.DeleteSfcProduceBusinesssBySfcInfoIdsCommand = new DeleteSfcProduceBusinesssBySfcInfoIdsCommand
            {
                SiteId = bo.SiteId,
                SfcInfoIds = sfcProduceEntities.Select(s => s.Id) //manuSfcEntities.Select(s => s.Id)
            };

            // 删除 manu_sfc_produce
            responseBo.DeletePhysicalByProduceIdsCommand = new DeletePhysicalByProduceIdsCommand
            {
                SiteId = bo.SiteId,
                Ids = sfcProduceEntities.Select(s => s.Id)
            };

            // 入库
            foreach (var sfcProduceEntity in sfcProduceEntities)
            {
                // 获取当前条码信息
                var manuSfcEntity = manuSFCEntities.FirstOrDefault(s => s.Id == sfcProduceEntity.SFCId);
                if (manuSfcEntity == null) continue;

                // 更新条码信息
                manuSfcEntity.UpdatedBy = updatedBy;
                manuSfcEntity.UpdatedOn = updatedOn;

                // 新增 wh_material_inventory
                responseBo.WhMaterialInventoryEntities.Add(new WhMaterialInventoryEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SupplierId = 0,//自制品 没有
                    MaterialId = sfcProduceEntity.ProductId,
                    MaterialBarCode = sfcProduceEntity.SFC,
                    Batch = "",//自制品 没有
                    MaterialType = MaterialInventoryMaterialTypeEnum.SelfMadeParts,
                    QuantityResidue = procMaterialEntity.Batch,
                    Status = WhMaterialInventoryStatusEnum.ToBeUsed,
                    Source = MaterialInventorySourceEnum.ManuComplete,
                    SiteId = bo.SiteId,
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                });

                // 新增 wh_material_standingbook
                responseBo.WhMaterialStandingbookEntities.Add(new WhMaterialStandingbookEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    MaterialCode = procMaterialEntity.MaterialCode,
                    MaterialName = procMaterialEntity.MaterialName,
                    MaterialVersion = procMaterialEntity.Version ?? "",
                    MaterialBarCode = sfcProduceEntity.SFC,
                    Batch = "",//自制品 没有
                    Quantity = procMaterialEntity.Batch,
                    Unit = procMaterialEntity.Unit ?? "",
                    Type = WhMaterialInventoryTypeEnum.ManuComplete,
                    Source = MaterialInventorySourceEnum.ManuComplete,
                    SiteId = bo.SiteId,
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                });
            }

            return responseBo;
        }

        /// <summary>
        /// 执行入库
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<JobResponseBo> ExecuteAsync(object obj)
        {
            JobResponseBo responseBo = new();
            if (obj is not SmiFinisheResponseBo data) return responseBo;

            // 更新条码状态
            if (data.SFCEntities.Any())
            {
                // manu_sfc 修改为完成 且入库
                responseBo.Rows += await _manuSfcRepository.UpdateRangeWithStatusCheckAsync(data.SFCEntities);

                // 未更新到全部需更新的数据，事务回滚
                if (data.SFCEntities.Count() > responseBo.Rows)
                {
                    responseBo.Rows = -1;
                    responseBo.Message = _localizationService.GetResource(nameof(ErrorCode.MES18216), data.SFCEntities.FirstOrDefault()!.SFC ?? "");
                    return responseBo;
                }
            }

            // 更新数据
            List<Task<int>> tasks = new()
            {
                // 删除 manu_sfc_produce
                _manuSfcProduceRepository.DeletePhysicalRangeByIdsSqlAsync(data.DeletePhysicalByProduceIdsCommand),

                // 删除 manu_sfc_produce_business
                _manuSfcProduceRepository.DeleteSfcProduceBusinessBySfcInfoIdsAsync(data.DeleteSfcProduceBusinesssBySfcInfoIdsCommand),

                // 入库
                _whMaterialInventoryRepository.InsertsAsync(data.WhMaterialInventoryEntities),
                _whMaterialStandingbookRepository.InsertsAsync(data.WhMaterialStandingbookEntities)
            };

            // 插入 manu_sfc_step
            if (data.SFCStepEntities.Any())
            {
                tasks.Add(_manuSfcStepRepository.InsertRangeAsync(data.SFCStepEntities));
            }

            var rowArray = await Task.WhenAll(tasks);
            responseBo.Rows += rowArray.Sum();

            return responseBo;
        }

        /// <summary>
        /// 执行后节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> AfterExecuteAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;
            return null;
        }

    }
}
