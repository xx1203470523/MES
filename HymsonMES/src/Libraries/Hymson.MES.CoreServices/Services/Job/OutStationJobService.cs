using Dapper;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.CoreServices.Services.NewJob
{
    /// <summary>
    /// 出站
    /// </summary>
    [Job("出站", JobTypeEnum.Standard)]
    public class OutStationJobService : IJobService
    {
        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;

        /// <summary>
        /// 仓储接口（生产工单）
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 仓储接口（上料信息）
        /// </summary>
        private readonly IManuFeedingRepository _manuFeedingRepository;

        /// <summary>
        /// 仓储接口（条码信息）
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

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
        /// 构造函数
        /// </summary>
        /// <param name="manuCommonService"></param>
        /// <param name="masterDataService"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="manuFeedingRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuSfcCirculationRepository"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="whMaterialStandingbookRepository"></param>
        public OutStationJobService(IManuCommonService manuCommonService,
            IMasterDataService masterDataService,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IManuFeedingRepository manuFeedingRepository,
            IManuSfcRepository manuSfcRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IWhMaterialStandingbookRepository whMaterialStandingbookRepository)
        {
            _manuCommonService = manuCommonService;
            _masterDataService = masterDataService;
            _planWorkOrderRepository = planWorkOrderRepository;
            _manuFeedingRepository = manuFeedingRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _whMaterialStandingbookRepository = whMaterialStandingbookRepository;
        }


        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<OutStationRequestBo>();
            if (bo == null) return default;

            // 待执行的命令
            OutStationResponseBo responseBo = new();

            // 获取生产条码信息
            var sfcProduceEntities = await bo.Proxy.GetValueAsync(_masterDataService.GetProduceEntitiesBySFCsAsync, bo);
            responseBo.SFCProduceEntities = sfcProduceEntities.AsList();
            if (responseBo.SFCProduceEntities == null || responseBo.SFCProduceEntities.Any() == false) return default;

            var firstProduceEntity = responseBo.SFCProduceEntities.FirstOrDefault();
            if (firstProduceEntity == null) return default;

            // 更新时间
            var updatedBy = bo.UserName;
            var updatedOn = HymsonClock.Now();

            // 读取条码信息
            var manuSfcEntities = await _masterDataService.GetManuSFCEntitiesWithNullCheck(bo);

            // 读取产品基础信息
            var procMaterialEntity = await _masterDataService.GetProcMaterialEntityWithNullCheck(firstProduceEntity.ProductId);

            // 读取当前工艺路线信息
            var procProcessRouteEntity = await _masterDataService.GetProcProcessRouteEntityWithNullCheck(firstProduceEntity.ProcessRouteId);

            // 合格品出站
            // 获取下一个工序（如果没有了，就表示完工）
            var nextProcedure = await bo.Proxy.GetValueAsync(_masterDataService.GetNextProcedureAsync, firstProduceEntity);
            responseBo.IsCompleted = nextProcedure == null;

            // 扣料
            //await func(sfcProduceEntity.ProductBOMId, sfcProduceEntity.ProcedureId);
            var initialMaterials = await bo.Proxy.GetValueAsync(_masterDataService.GetInitialMaterialsAsync, firstProduceEntity);
            if (initialMaterials == null) return default;

            // 物料ID集合
            var materialIds = initialMaterials.Select(s => s.MaterialId);

            // 读取物料加载数据（批量）
            var allFeedingEntities = await bo.Proxy.GetValueAsync(_manuFeedingRepository.GetByResourceIdAndMaterialIdsWithOutZeroAsync, new GetByResourceIdAndMaterialIdsQuery
            {
                ResourceId = firstProduceEntity.ResourceId ?? 0,
                MaterialIds = materialIds
            });

            // 通过物料分组
            var manuFeedingsDictionary = allFeedingEntities?.ToLookup(w => w.ProductId).ToDictionary(d => d.Key, d => d);

            // 过滤扣料集合
            List<UpdateQtyByIdCommand> updates = new();
            List<ManuSfcCirculationEntity> adds = new();
            foreach (var materialBo in initialMaterials)
            {
                if (manuFeedingsDictionary == null) continue;

                // 需扣减数量 = 用量 * 损耗 * 消耗系数 ÷ 100
                decimal residue = materialBo.Usages;
                if (materialBo.Loss.HasValue == true && materialBo.Loss > 0) residue *= materialBo.Loss.Value;
                if (materialBo.ConsumeRatio > 0) residue *= (materialBo.ConsumeRatio / 100);

                // 收集方式是批次
                if (materialBo.DataCollectionWay == MaterialSerialNumberEnum.Batch)
                {
                    // 进行扣料
                    _masterDataService.DeductMaterialQty(ref updates, ref adds, ref residue, firstProduceEntity, manuFeedingsDictionary, materialBo, materialBo);
                    continue;
                }

                // 2.确认主物料的收集方式，不是"批次"就结束（不对该物料进行扣料）
                if (materialBo.SerialNumber != MaterialSerialNumberEnum.Batch) continue;

                // 进行扣料
                _masterDataService.DeductMaterialQty(ref updates, ref adds, ref residue, firstProduceEntity, manuFeedingsDictionary, materialBo, materialBo);
            }
            responseBo.UpdateQtyByIdCommands = updates;
            responseBo.ManuSfcCirculationEntities = adds;

            // 组装（出站步骤数据）
            responseBo.SFCProduceEntities.ForEach(sfcProduceEntity =>
            {
                // 初始化步骤
                var stepEntity = new ManuSfcStepEntity
                {
                    // 插入 manu_sfc_step 状态为出站（默认值）
                    Operatetype = ManuSfcStepTypeEnum.OutStock,
                    CurrentStatus = SfcProduceStatusEnum.Activity,
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
                };

                // 已完工
                if (responseBo.IsCompleted)
                {
                    stepEntity.Operatetype = procProcessRouteEntity.Type == ProcessRouteTypeEnum.UnqualifiedRoute ? ManuSfcStepTypeEnum.RepairComplete : ManuSfcStepTypeEnum.OutStock;    // TODO 这里的状态？？
                    stepEntity.CurrentStatus = SfcProduceStatusEnum.Complete;  // TODO 这里的状态？？
                }
                // 未完工
                else
                {
                    // 更新时间
                    sfcProduceEntity.UpdatedBy = updatedBy;
                    sfcProduceEntity.UpdatedOn = updatedOn;
                    sfcProduceEntity.Status = SfcProduceStatusEnum.lineUp;
                    if (nextProcedure != null)
                    {
                        sfcProduceEntity.ProcedureId = nextProcedure.Id;
                        // 不置空的话，进站时，可能校验不通过
                        sfcProduceEntity.ResourceId = null;
                    }
                }

                responseBo.SFCStepEntities.Add(stepEntity);
            });

            // 已完工，且只有"生产主工艺路线"，出站时才走下面流程
            responseBo.ProcessRouteType = procProcessRouteEntity.Type;
            if (responseBo.IsCompleted && responseBo.ProcessRouteType == ProcessRouteTypeEnum.ProductionRoute)
            {
                // 删除 manu_sfc_produce_business
                responseBo.DeleteSfcProduceBusinesssBySfcInfoIdsCommand = new DeleteSfcProduceBusinesssBySfcInfoIdsCommand
                {
                    SiteId = bo.SiteId,
                    SfcInfoIds = responseBo.SFCProduceEntities.Select(s => s.Id) //manuSfcEntities.Select(s => s.Id)
                };

                // 更新完工数量
                responseBo.UpdateQtyCommand = new UpdateQtyCommand
                {
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn,
                    WorkOrderId = firstProduceEntity.WorkOrderId,
                    Qty = responseBo.SFCProduceEntities.Count
                };

                // 删除 manu_sfc_produce
                responseBo.DeletePhysicalBySfcsCommand = new DeletePhysicalBySfcsCommand
                {
                    SiteId = bo.SiteId,
                    Sfcs = responseBo.SFCProduceEntities.Select(s => s.SFC)
                };

                // manu_sfc_info 修改为完成 且入库
                responseBo.MultiSFCUpdateStatusCommand = new MultiSFCUpdateStatusCommand
                {
                    SiteId = bo.SiteId,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn,
                    Status = SfcStatusEnum.Complete,
                    SFCs = manuSfcEntities.Select(s => s.SFC)

                };

                // 入库
                responseBo.SFCProduceEntities.ForEach(sfcProduceEntity =>
                {
                    // 新增 wh_material_inventory
                    responseBo.WhMaterialInventoryEntities.Add(new WhMaterialInventoryEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SupplierId = 0,//自制品 没有
                        MaterialId = sfcProduceEntity.ProductId,
                        MaterialBarCode = sfcProduceEntity.SFC,
                        Batch = "",//自制品 没有
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
            if (obj is not OutStationResponseBo data) return responseBo;

            // 更新数据
            List<Task<int>> tasks = new();
            //using var trans = new TransactionScope();

            // 更新物料库存
            if (data.UpdateQtyByIdCommands.Any())
            {
                //tasks.Add(_manuFeedingRepository.UpdateQtyByIdAsync(data.UpdateQtyByIdCommands));

                /*
                foreach (var item in data.UpdateQtyByIdCommands)
                {
                    responseBo.Rows += await _manuFeedingRepository.UpdateQtyByIdAsync(item);
                }
                */
                responseBo.Rows += await _manuFeedingRepository.UpdateQtyByIdAsync(data.UpdateQtyByIdCommands);

                // 未更新到全部需更新的数据，事务回滚
                if (data.UpdateQtyByIdCommands.Count() > responseBo.Rows)
                {
                    responseBo.Rows = -1;
                    return responseBo;
                }
            }

            // 添加流转记录
            if (data.ManuSfcCirculationEntities.Any())
            {
                tasks.Add(_manuSfcCirculationRepository.InsertRangeAsync(data.ManuSfcCirculationEntities));
            }

            // 插入 manu_sfc_step
            if (data.SFCStepEntities.Any())
            {
                tasks.Add(_manuSfcStepRepository.InsertRangeAsync(data.SFCStepEntities));
            }

            // 完工
            if (data.IsCompleted)
            {
                // 只有"生产主工艺路线"，出站时才走下面流程
                if (data.ProcessRouteType == ProcessRouteTypeEnum.ProductionRoute)
                {
                    // 删除 manu_sfc_produce
                    tasks.Add(_manuSfcProduceRepository.DeletePhysicalRangeAsync(data.DeletePhysicalBySfcsCommand));

                    // 删除 manu_sfc_produce_business
                    //tasks.Add(_manuSfcProduceRepository.DeleteSfcProduceBusinessBySfcInfoIdsAsync(data.DeleteSfcProduceBusinesssBySfcInfoIdsCommand));

                    // 更新完工数量
                    tasks.Add(_planWorkOrderRepository.UpdateFinishProductQuantityByWorkOrderIdAsync(data.UpdateQtyCommand));

                    // manu_sfc_info 修改为完成 且入库
                    tasks.Add(_manuSfcRepository.MultiUpdateSfcStatusAsync(data.MultiSFCUpdateStatusCommand));

                    //// 2023.05.29 克明说不在这里更新完成时间
                    //// 更新工单统计表的 RealEnd
                    //rows += await _planWorkOrderRepository.UpdatePlanWorkOrderRealEndByWorkOrderIdAsync(new UpdateWorkOrderRealTimeCommand
                    //{
                    //    UpdatedOn = sfcProduceEntity.UpdatedOn,
                    //    UpdatedBy = sfcProduceEntity.UpdatedBy,
                    //    WorkOrderIds = new long[] { sfcProduceEntity.WorkOrderId }
                    //});

                    // 入库
                    tasks.Add(_whMaterialInventoryRepository.InsertsAsync(data.WhMaterialInventoryEntities));
                    tasks.Add(_whMaterialStandingbookRepository.InsertsAsync(data.WhMaterialStandingbookEntities));
                }
            }
            // 未完工
            else
            {
                // 修改 manu_sfc_produce 为排队, 工序修改为下一工序的id
                tasks.Add(_manuSfcProduceRepository.UpdateRangeAsync(data.SFCProduceEntities));
            }

            var rowArray = await Task.WhenAll(tasks);
            responseBo.Rows += rowArray.Sum();
            //trans.Complete();

            return responseBo;
        }

    }
}
