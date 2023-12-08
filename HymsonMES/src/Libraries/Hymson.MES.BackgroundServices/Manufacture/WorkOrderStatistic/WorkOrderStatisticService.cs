using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcStep.Query;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.Snowflake;
using Hymson.Utils.Tools;
using Hymson.WaterMark;

namespace Hymson.MES.BackgroundServices.Manufacture
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkOrderStatisticService : IWorkOrderStatisticService
    {
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
        /// <param name="waterMarkService"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuWorkOrderSFCRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        public WorkOrderStatisticService(IWaterMarkService waterMarkService,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuWorkOrderSFCRepository manuWorkOrderSFCRepository,
            IPlanWorkOrderRepository planWorkOrderRepository)
        {
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
            var manuSfcStepList = await _manuSfcStepRepository.GetListByStartWaterMarkIdAsync(new ManuSfcStepStatisticQuery
            {
                StartWaterMarkId = waterMarkId,
                Rows = limitCount
            });
            if (manuSfcStepList == null || !manuSfcStepList.Any()) return;

            var user = $"{BusinessKey.WorkOrderStatistic}作业";
            var manuSfcStepDic = manuSfcStepList.ToLookup(x => new SingleWorkOrderSFCBo
            {
                SiteId = x.SiteId,
                WorkOrderId = x.WorkOrderId,
                SFC = x.SFC
            }).ToDictionary(d => d.Key, d => d);

            List<ManuWorkOrderSFCEntity> inStationSteps = new();
            List<ManuWorkOrderSFCEntity> outStationSteps = new();
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
                    entity.SFCStatus = SfcStatusEnum.lineUp;
                    entity.CreatedOn = stepsOfLineUp.Min(m => m.CreatedOn);
                    entity.UpdatedOn = entity.CreatedOn;
                    inStationSteps.Add(entity);
                }

                // 出站
                var stepsOfComplete = item.Value.Where(a => a.AfterOperationStatus == SfcStatusEnum.Complete);
                if (stepsOfComplete.Any())
                {
                    entity.SFCStatus = SfcStatusEnum.Complete;
                    entity.CreatedOn = stepsOfComplete.Max(m => m.CreatedOn);
                    entity.UpdatedOn = entity.CreatedOn;
                    outStationSteps.Add(entity);
                }
            }

            // 保存工单条码记录
            await _manuWorkOrderSFCRepository.InsertRangeAsync(inStationSteps);
            await _manuWorkOrderSFCRepository.RepalceRangeAsync(outStationSteps);

            // 通过工单对条码进行分组
            var manuSfcStepForWorkOrderDic = manuSfcStepList.ToLookup(x => x.WorkOrderId).ToDictionary(d => d.Key, d => d);

            List<UpdateQtyByWorkOrderIdCommand> updateInputQtyCommands = new();
            List<UpdateQtyByWorkOrderIdCommand> updateFinishQtyCommands = new();
            foreach (var item in manuSfcStepForWorkOrderDic)
            {
                var records = await _manuWorkOrderSFCRepository.GetEntitiesAsync(new EntityByWorkOrderIdQuery { WorkOrderId = item.Key });

                // 投入数量（数量那里分组，只是为了去掉重复进站）
                updateInputQtyCommands.Add(new UpdateQtyByWorkOrderIdCommand
                {
                    WorkOrderId = item.Key,
                    Qty = records.Count(w => w.SFCStatus == SfcStatusEnum.lineUp),
                    UpdatedBy = user,
                    UpdatedOn = item.Value.Min(w => w.CreatedOn)
                });

                // 产出数量（数量那里分组，只是为了去掉重复出站）
                updateFinishQtyCommands.Add(new UpdateQtyByWorkOrderIdCommand
                {
                    WorkOrderId = item.Key,
                    Qty = records.Count(w => w.SFCStatus == SfcStatusEnum.Complete),
                    UpdatedBy = user,
                    UpdatedOn = item.Value.Max(w => w.CreatedOn)
                });
            }

            using var trans = TransactionHelper.GetTransactionScope();

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
