using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.Snowflake;
using Hymson.Utils.Tools;
using Hymson.WaterMark;
using Microsoft.Extensions.Logging;

namespace Hymson.MES.BackgroundServices.Manufacture
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkOrderStatisticService : IWorkOrderStatisticService
    {
        /// <summary>
        /// 日志服务接口
        /// </summary>
        private readonly ILogger<WorkOrderStatisticService> _logger;

        /// <summary>
        /// 服务接口（水位）
        /// </summary>
        public readonly IWaterMarkService _waterMarkService;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 仓储接口（工单条码记录）
        /// </summary>
        private readonly IManuWorkOrderSFCRepository _manuWorkOrderSFCRepository;

        /// <summary>
        /// 统计服务
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="waterMarkService"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuWorkOrderSFCRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        public WorkOrderStatisticService(ILogger<WorkOrderStatisticService> logger,
            IWaterMarkService waterMarkService,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuWorkOrderSFCRepository manuWorkOrderSFCRepository,
            IPlanWorkOrderRepository planWorkOrderRepository)
        {
            _logger = logger;
            _waterMarkService = waterMarkService;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuWorkOrderSFCRepository = manuWorkOrderSFCRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
        }

        /// <summary>
        /// 执行统计
        /// </summary>
        /// <param name="limitCount"></param>
        /// <returns></returns>
        public async Task ExecuteAsync(int limitCount = 1000)
        {
            var waterMarkId = await _waterMarkService.GetWaterMarkAsync(BusinessKey.WorkOrderStatistic);

            // 获取步骤表数据
            var manuSfcStepList = await _manuSfcStepRepository.GetListByStartWaterMarkIdAsync(new EntityByWaterMarkQuery
            {
                StartWaterMarkId = waterMarkId,
                Rows = limitCount
            });
            if (manuSfcStepList == null || !manuSfcStepList.Any())
            {
                _logger.LogDebug($"工单统计 -> 没有新产生的的步骤数据！waterMarkId:{waterMarkId}");
                return;
            }

            var user = $"{BusinessKey.WorkOrderStatistic}作业";

            // 相同工单和条码的数据只记录一条记录
            var manuSfcStepDic = manuSfcStepList.ToLookup(x => new SingleWorkOrderSFCBo
            {
                SiteId = x.SiteId,
                WorkOrderId = x.WorkOrderId,
                SFC = x.SFC
            }).ToDictionary(d => d.Key, d => d);

            List<ManuWorkOrderSFCEntity> inStationSteps = new();
            List<ManuWorkOrderSFCEntity> outStationSteps = new();
            List<ManuWorkOrderSFCEntity> completeSteps = new();
            List<ManuWorkOrderSFCEntity> deductionSteps = new();
            foreach (var item in manuSfcStepDic)
            {
                var entity = new ManuWorkOrderSFCEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = item.Key.SiteId,
                    WorkOrderId = item.Key.WorkOrderId,
                    SFC = item.Key.SFC,
                    CreatedBy = user,
                    UpdatedBy = user
                };

                // 进站
                var stepsOfLineUp = item.Value.Where(a => a.CurrentStatus == SfcStatusEnum.lineUp);
                if (stepsOfLineUp.Any())
                {
                    entity.Status = SfcStatusEnum.lineUp;
                    entity.CreatedOn = stepsOfLineUp.Min(m => m.CreatedOn);
                    entity.UpdatedOn = entity.CreatedOn;
                    inStationSteps.Add(entity);
                }

                // 活动中（步骤控制可能直接改为活动中）
                var stepsOfActivity = item.Value.Where(a => a.CurrentStatus == SfcStatusEnum.Activity);
                if (stepsOfActivity.Any())
                {
                    entity.Status = SfcStatusEnum.lineUp;   // 这里为什么是排队中，是因为步骤控制操作的条码有出站，却没有进站。
                    entity.CreatedOn = stepsOfActivity.Min(m => m.CreatedOn);
                    entity.UpdatedOn = entity.CreatedOn;
                    outStationSteps.Add(entity);
                }

                // 出站
                var stepsOfComplete = item.Value.Where(a => a.AfterOperationStatus == SfcStatusEnum.Complete);
                if (stepsOfComplete.Any())
                {
                    entity.Status = SfcStatusEnum.Complete;
                    entity.CreatedOn = stepsOfComplete.Max(m => m.CreatedOn);
                    entity.UpdatedOn = entity.CreatedOn;
                    completeSteps.Add(entity);
                }

                // 需要扣减统计的类型
                var stepsOfDeduction = item.Value.Where(a => ManuSfcStatus.ForbidSfcStatuss.Contains(a.CurrentStatus));
                if (stepsOfDeduction.Any())
                {
                    entity.Status = SfcStatusEnum.Detachment; // 这里的状态其实不重要，因为对应的条码记录均会被删除
                    entity.CreatedOn = stepsOfDeduction.Max(m => m.CreatedOn);
                    entity.UpdatedOn = entity.CreatedOn;
                    deductionSteps.Add(entity);
                }
            }

            // 保存工单条码记录
            var insertStationSteps = inStationSteps.Concat(outStationSteps);

            await _manuWorkOrderSFCRepository.IgnoreRangeAsync(insertStationSteps);
            await _manuWorkOrderSFCRepository.RepalceRangeAsync(completeSteps);

            await _manuWorkOrderSFCRepository.DeleteRangeAsync(deductionSteps);

            // 通过工单对条码进行分组，看哪些工单需要更新
            var manuSfcStepForWorkOrderDic = manuSfcStepList.ToLookup(x => x.WorkOrderId).ToDictionary(d => d.Key, d => d);

            List<UpdateQtyByWorkOrderIdCommand> updateInputQtyCommands = new();
            List<UpdateQtyByWorkOrderIdCommand> updateFinishQtyCommands = new();
            foreach (var item in manuSfcStepForWorkOrderDic)
            {
                // 根据工单取得记录表的所有条码数据
                var records = await _manuWorkOrderSFCRepository.GetEntitiesAsync(new EntityByWorkOrderIdQuery { WorkOrderId = item.Key });

                // 投入数量（数量那里分组，只是为了去掉重复进站）
                updateInputQtyCommands.Add(new UpdateQtyByWorkOrderIdCommand
                {
                    WorkOrderId = item.Key,
                    Qty = records.Count(w => w.Status == SfcStatusEnum.lineUp),
                    UpdatedBy = user,
                    UpdatedOn = item.Value.Min(w => w.CreatedOn)
                });

                // 产出数量（数量那里分组，只是为了去掉重复出站）
                updateFinishQtyCommands.Add(new UpdateQtyByWorkOrderIdCommand
                {
                    WorkOrderId = item.Key,
                    Qty = records.Count(w => w.Status == SfcStatusEnum.Complete),
                    UpdatedBy = user,
                    UpdatedOn = item.Value.Max(w => w.CreatedOn)
                });
            }

            using var trans = TransactionHelper.GetTransactionScope(timeout: 60);

            // 更新工单的投入数量
            await _planWorkOrderRepository.UpdateInputQtyByWorkOrderIdsAsync(updateInputQtyCommands);

            // 更新工单的产出数量
            await _planWorkOrderRepository.UpdateFinishProductQuantityByWorkOrderIdsAsync(updateFinishQtyCommands);

            // 更新水位
            await _waterMarkService.RecordWaterMarkAsync(BusinessKey.WorkOrderStatistic, manuSfcStepList.Max(x => x.Id));
            trans.Complete();
        }

    }
}
