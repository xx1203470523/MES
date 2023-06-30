using Dapper;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Query;
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
        /// 仓储接口（条码信息）
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 仓储接口（上料信息）
        /// </summary>
        private readonly IManuFeedingRepository _manuFeedingRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manuCommonService"></param>
        /// <param name="masterDataService"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuFeedingRepository"></param>
        public OutStationJobService(IManuCommonService manuCommonService,
            IMasterDataService masterDataService,
            IManuSfcRepository manuSfcRepository,
            IManuFeedingRepository manuFeedingRepository)
        {
            _manuCommonService = manuCommonService;
            _masterDataService = masterDataService;
            _manuSfcRepository = manuSfcRepository;
            _manuFeedingRepository = manuFeedingRepository;
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
            if ((param is OutStationRequestBo bo) == false) return default;

            // 获取生产条码信息
            var sfcProduceEntities = await param.Proxy.GetValueAsync(_masterDataService.GetProduceEntitiesBySFCsAsync, bo);
            var entities = sfcProduceEntities.AsList();
            if (entities == null || entities.Any() == false) return default;

            var firstProduceEntity = entities.FirstOrDefault();
            if (firstProduceEntity == null) return default;

            // 组装（出站数据）
            List<ManuSfcStepEntity> sfcStepEntities = new();
            entities.ForEach(sfcProduceEntity =>
            {
                // 更新时间
                sfcProduceEntity.UpdatedBy = bo.UserName;
                sfcProduceEntity.UpdatedOn = HymsonClock.Now();

                // 初始化步骤
                sfcStepEntities.Add(new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = sfcProduceEntity.SiteId,
                    SFC = sfcProduceEntity.SFC,
                    ProductId = sfcProduceEntity.ProductId,
                    WorkOrderId = sfcProduceEntity.WorkOrderId,
                    WorkCenterId = sfcProduceEntity.WorkCenterId,
                    ProductBOMId = sfcProduceEntity.ProductBOMId,
                    ProcedureId = sfcProduceEntity.ProcedureId,
                    Qty = sfcProduceEntity.Qty,
                    EquipmentId = sfcProduceEntity.EquipmentId,
                    ResourceId = sfcProduceEntity.ResourceId,
                    CreatedBy = sfcProduceEntity.UpdatedBy,
                    CreatedOn = sfcProduceEntity.UpdatedOn.Value,
                    UpdatedBy = sfcProduceEntity.UpdatedBy,
                    UpdatedOn = sfcProduceEntity.UpdatedOn.Value,
                });
            });

            // 合格品出站
            // 获取下一个工序（如果没有了，就表示完工）
            var nextProcedure = await param.Proxy.GetValueAsync(_masterDataService.GetNextProcedureAsync, firstProduceEntity);

            // 扣料
            //await func(sfcProduceEntity.ProductBOMId, sfcProduceEntity.ProcedureId);
            var initialMaterials = await param.Proxy.GetValueAsync(_masterDataService.GetInitialMaterialsAsync, firstProduceEntity);
            if (initialMaterials == null) return default;

            // 物料ID集合
            var materialIds = initialMaterials.Select(s => s.MaterialId);

            // 读取物料加载数据（批量）
            var allFeedingEntities = await param.Proxy.GetValueAsync(_manuFeedingRepository.GetByResourceIdAndMaterialIdsWithOutZeroAsync, new GetByResourceIdAndMaterialIdsQuery
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
                if (materialBo.Loss.HasValue == true && materialBo.Loss > 0) residue *= (materialBo.Loss.Value / 100);
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

            // manu_sfc_info 修改为完成 且入库
            // 条码信息
            var sfcInfo = await _manuSfcRepository.GetManuSfcEntitiesAsync(new ManuSfcQuery
            {
                SiteId = bo.SiteId,
                SFCs = bo.SFCs
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES17104));

            // 读取产品基础信息
            var procMaterialEntity = await _masterDataService.GetProcMaterialEntityWithNullCheck(firstProduceEntity.ProductId);

            // 读取当前工艺路线信息
            var currentProcessRoute = await _masterDataService.GetProcProcessRouteEntityWithNullCheck(firstProduceEntity.ProcessRouteId);

            // TODO
            return null;
        }

        /// <summary>
        /// 执行入库
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(object obj)
        {
            /*
            // 更新数据
            using var trans = TransactionHelper.GetTransactionScope();
            List<Task> tasks = new();

            // 更新物料库存
            if (updates.Any() == true)
            {
                rows += await _manuFeedingRepository.UpdateQtyByIdAsync(updates);

                // 未更新到全部需更新的数据，事务回滚
                if (updates.Count > rows)
                {
                    trans.Dispose();
                    return 0;
                }
            }

            // 添加流转记录
            if (adds.Any() == true)
            {
                var manuSfcCirculationInsertRangeTask = _manuSfcCirculationRepository.InsertRangeAsync(adds);
                tasks.Add(manuSfcCirculationInsertRangeTask);
            }

            // 完工
            if (nextProcedure == null)
            {
                // 插入 manu_sfc_step 状态为 完成
                sfcStep.Operatetype = currentProcessRoute.Type == ProcessRouteTypeEnum.UnqualifiedRoute ? ManuSfcStepTypeEnum.RepairComplete : ManuSfcStepTypeEnum.OutStock;    // TODO 这里的状态？？
                sfcStep.CurrentStatus = SfcProduceStatusEnum.Complete;  // TODO 这里的状态？？
                var manuSfcStepInsertTask = _manuSfcStepRepository.InsertAsync(sfcStep);
                tasks.Add(manuSfcStepInsertTask);

                // 只有"生产主工艺路线"，出站时才走下面流程
                if (currentProcessRoute.Type == ProcessRouteTypeEnum.ProductionRoute)
                {
                    // 删除 manu_sfc_produce
                    var manuSfcProduceDeletePhysicalTask = _manuSfcProduceRepository.DeletePhysicalAsync(new DeletePhysicalBySfcCommand()
                    {
                        SiteId = _currentSite.SiteId ?? 0,
                        Sfc = sfcProduceEntity.SFC
                    });
                    tasks.Add(manuSfcProduceDeletePhysicalTask);

                    // 删除 manu_sfc_produce_business
                    var manuSfcProduceDeleteSfcProduceBusinessBySfcInfoIdTask = _manuSfcProduceRepository.DeleteSfcProduceBusinessBySfcInfoIdAsync(new DeleteSfcProduceBusinesssBySfcInfoIdCommand
                    {
                        SiteId = sfcProduceEntity.SiteId,
                        SfcInfoId = sfcInfo.Id
                    });
                    tasks.Add(manuSfcProduceDeleteSfcProduceBusinessBySfcInfoIdTask);

                    // 更新完工数量
                    var planWorkOrderUpdateFinishProductQuantityByWorkOrderIdTask = _planWorkOrderRepository.UpdateFinishProductQuantityByWorkOrderIdAsync(new UpdateQtyCommand
                    {
                        UpdatedBy = sfcProduceEntity.UpdatedBy,
                        UpdatedOn = sfcProduceEntity.UpdatedOn,
                        WorkOrderId = sfcProduceEntity.WorkOrderId,
                        Qty = 1,
                    });
                    tasks.Add(planWorkOrderUpdateFinishProductQuantityByWorkOrderIdTask);

                    // 更新状态
                    sfcInfo.Status = SfcStatusEnum.Complete;
                    sfcInfo.UpdatedBy = sfcProduceEntity.UpdatedBy;
                    sfcInfo.UpdatedOn = sfcProduceEntity.UpdatedOn;
                    var manuSfcUpdateTask = _manuSfcRepository.UpdateAsync(sfcInfo);
                    tasks.Add(manuSfcUpdateTask);

                    //// 2023.05.29 克明说不在这里更新完成时间
                    //// 更新工单统计表的 RealEnd
                    //rows += await _planWorkOrderRepository.UpdatePlanWorkOrderRealEndByWorkOrderIdAsync(new UpdateWorkOrderRealTimeCommand
                    //{
                    //    UpdatedOn = sfcProduceEntity.UpdatedOn,
                    //    UpdatedBy = sfcProduceEntity.UpdatedBy,
                    //    WorkOrderIds = new long[] { sfcProduceEntity.WorkOrderId }
                    //});

                    // 入库
                    SaveToWarehouseAsync(ref tasks, sfcProduceEntity, procMaterialEntity);
                }
            }
            // 未完工
            else
            {
                // 修改 manu_sfc_produce 为排队, 工序修改为下一工序的id
                sfcProduceEntity.Status = SfcProduceStatusEnum.lineUp;
                sfcProduceEntity.ProcedureId = nextProcedure.Id;
                var manuSfcProduceUpdateTask = _manuSfcProduceRepository.UpdateAsync(sfcProduceEntity);
                tasks.Add(manuSfcProduceUpdateTask);

                // 插入 manu_sfc_step 状态为 进站
                sfcStep.Operatetype = ManuSfcStepTypeEnum.OutStock;
                var manuSfcStepInsertTask = _manuSfcStepRepository.InsertAsync(sfcStep);
                tasks.Add(manuSfcStepInsertTask);
            }

            await Task.WhenAll(tasks);
            trans.Complete();
            */

            await Task.CompletedTask;
            return 0;
        }

    }
}
