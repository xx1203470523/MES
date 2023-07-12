using Dapper;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.CoreServices.Services.NewJob
{
    /// <summary>
    /// 进站
    /// </summary>
    [Job("进站", JobTypeEnum.Standard)]
    public class InStationJobService : IJobService
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
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 仓储接口（工单信息）
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;



        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manuCommonService"></param>
        /// <param name="masterDataService"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        public InStationJobService(IManuCommonService manuCommonService,
            IMasterDataService masterDataService,
            IManuSfcRepository manuSfcRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IPlanWorkOrderRepository planWorkOrderRepository)
        {
            _manuCommonService = manuCommonService;
            _masterDataService = masterDataService;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
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
        /// <returns></returns>00
        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<InStationRequestBo>();
            if (bo == null) return default;

            // 获取生产条码信息
            var sfcProduceEntities = await bo.Proxy.GetValueAsync(_masterDataService.GetProduceEntitiesBySFCsAsync, bo);

            if (sfcProduceEntities == null || sfcProduceEntities.Any() == false) return default;
            var entities = sfcProduceEntities.AsList();

            var firstProduceEntity = entities.FirstOrDefault();
            if (firstProduceEntity == null) return default;

            // 更新时间
            var updatedBy = bo.UserName;
            var updatedOn = HymsonClock.Now();

            // 检查是否首工序
            var isFirstProcedure = await bo.Proxy.GetValueAsync(async parameters =>
            {
                var processRouteId = (long)parameters[0];
                var procedureId = (long)parameters[1];
                return await _masterDataService.IsFirstProcedureAsync(processRouteId, procedureId);
            }, new object[] { firstProduceEntity.ProcessRouteId, firstProduceEntity.ProcedureId });

            // 获取当前工序信息
            var procedureEntity = await _masterDataService.GetProcProcedureEntityWithNullCheck(firstProduceEntity.ProcedureId);

            // 组装（进站数据）
            List<ManuSfcStepEntity> sfcStepEntities = new();
            List<UpdateWorkOrderRealTimeCommand> updateWorkOrderRealTimeCommands = new();
            List<UpdateQtyCommand> updateQtyCommands = new();
            MultiSfcUpdateIsUsedCommand sfcUpdateIsUsedCommand = new();
            entities.ForEach(sfcProduceEntity =>
            {
                // 检查是否测试工序
                if (procedureEntity.Type == ProcedureTypeEnum.Test)
                {
                    // 超过复投次数，标识为NG
                    if (firstProduceEntity.RepeatedCount > procedureEntity.Cycle) throw new CustomerValidationException(nameof(ErrorCode.MES16036));
                    firstProduceEntity.RepeatedCount++;
                }

                sfcProduceEntity.ProcedureId = bo.ProcedureId;
                sfcProduceEntity.ResourceId = bo.ResourceId;

                // 更新状态，将条码由"排队"改为"活动"
                sfcProduceEntity.Status = SfcProduceStatusEnum.Activity;
                sfcProduceEntity.UpdatedBy = bo.UserName;
                sfcProduceEntity.UpdatedOn = HymsonClock.Now();

                // 更新工单统计表的 RealStart
                updateWorkOrderRealTimeCommands.Add(new UpdateWorkOrderRealTimeCommand
                {
                    UpdatedOn = sfcProduceEntity.UpdatedOn,
                    UpdatedBy = sfcProduceEntity.UpdatedBy,
                    WorkOrderIds = new long[] { sfcProduceEntity.WorkOrderId }
                });

                // 如果是首工序
                if (isFirstProcedure == true)
                {
                    // 更新工单的 InputQty
                    updateQtyCommands.Add(new UpdateQtyCommand
                    {
                        UpdatedBy = sfcProduceEntity.UpdatedBy,
                        UpdatedOn = sfcProduceEntity.UpdatedOn,
                        WorkOrderId = sfcProduceEntity.WorkOrderId,
                        Qty = 1,
                    });
                }

                // 初始化步骤
                sfcStepEntities.Add(new ManuSfcStepEntity
                {
                    Operatetype = ManuSfcStepTypeEnum.InStock,  // 状态为 进站
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
            });

            if (isFirstProcedure == true)
            {
                // 修改条码使用状态为"已使用"
                sfcUpdateIsUsedCommand = new MultiSfcUpdateIsUsedCommand
                {
                    SiteId = bo.SiteId,
                    SFCs = entities.Select(s => s.SFC),
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn,
                    IsUsed = YesOrNoEnum.Yes
                };
            }

            return new InStationResponseBo
            {
                IsFirstProcedure = isFirstProcedure,
                SFCProduceEntities = entities,
                UpdateWorkOrderRealTimeCommands = updateWorkOrderRealTimeCommands,
                UpdateQtyCommands = updateQtyCommands,
                SFCStepEntities = sfcStepEntities,
                MultiSfcUpdateIsUsedCommand = sfcUpdateIsUsedCommand
            };
        }

        /// <summary>
        /// 执行入库
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<JobResponseBo> ExecuteAsync(object obj)
        {
            JobResponseBo responseBo = new();
            if (obj is not InStationResponseBo data) return responseBo;

            // 更新数据
            List<Task> tasks = new();

            // 更改状态
            responseBo.Rows += await _manuSfcProduceRepository.UpdateRangeWithStatusCheckAsync(data.SFCProduceEntities);

            // 未更新到数据，事务回滚
            if (responseBo.Rows <= 0)
            {
                // 这里在外层会回滚事务
                responseBo.Rows = -1;
                return responseBo;
            }

            // 更新工单统计表的 RealStart
            var updatePlanWorkOrderRealStartByWorkOrderIdTask = _planWorkOrderRepository.UpdatePlanWorkOrderRealStartByWorkOrderIdAsync(data.UpdateWorkOrderRealTimeCommands);
            tasks.Add(updatePlanWorkOrderRealStartByWorkOrderIdTask);

            // 插入 manu_sfc_step 状态为 进站
            var manuSfcStepTask = _manuSfcStepRepository.InsertRangeAsync(data.SFCStepEntities);
            tasks.Add(manuSfcStepTask);

            // 如果是首工序
            if (data.IsFirstProcedure == true)
            {
                // 更新工单的 InputQty
                var updateInputQtyByWorkOrderIdTask = _planWorkOrderRepository.UpdateInputQtyByWorkOrderIdAsync(data.UpdateQtyCommands);
                tasks.Add(updateInputQtyByWorkOrderIdTask);

                // 修改条码使用状态为"已使用"
                var multiUpdateSfcIsUsedTask = _manuSfcRepository.MultiUpdateSfcIsUsedAsync(data.MultiSfcUpdateIsUsedCommand);
                tasks.Add(multiUpdateSfcIsUsedTask);
            }

            await Task.WhenAll(tasks);
            return responseBo;
        }

    }
}
