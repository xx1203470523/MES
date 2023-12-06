using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcStep.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
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
        /// 统计服务
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="waterMarkService"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        public WorkOrderStatisticService(IWaterMarkService waterMarkService,
            IManuSfcStepRepository manuSfcStepRepository,
            IPlanWorkOrderRepository planWorkOrderRepository)
        {
            _waterMarkService = waterMarkService;
            _manuSfcStepRepository = manuSfcStepRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
        }

        /// <summary>
        /// 执行统计
        /// </summary>
        /// <param name="limitCount"></param>
        /// <returns></returns>
        public async Task ExecuteAsync(int limitCount = 500)
        {
            var waterMarkId = await _waterMarkService.GetWaterMarkAsync(BusinessKey.WorkOrderStatistic);

            // 获取步骤表数据
            var manuSfcStepList = await _manuSfcStepRepository.GetListByStartWaterMarkIdAsync(new ManuSfcStepStatisticQuery
            {
                StartwaterMarkId = waterMarkId,
                Rows = limitCount
            });
            if (manuSfcStepList == null || !manuSfcStepList.Any()) return;

            var user = "WorkOrderStatisticService";
            List<UpdateQtyByWorkOrderIdCommand> updateInputQtyCommands = new();
            List<UpdateQtyByWorkOrderIdCommand> updateFinishQtyCommands = new();
            var manuSfcStepDic = manuSfcStepList.ToLookup(x => x.WorkOrderId).ToDictionary(d => d.Key, d => d);
            foreach (var item in manuSfcStepDic)
            {
                // 投入数量（重复首工序进站时，要注意数量扣减）
                updateInputQtyCommands.Add(new UpdateQtyByWorkOrderIdCommand
                {
                    WorkOrderId = item.Key,
                    Qty = item.Value.Count(w => w.CurrentStatus == SfcStatusEnum.lineUp && w.Remark == "FirstProcedureInStation"),
                    UpdatedBy = user,
                    UpdatedOn = item.Value.Min(w => w.CreatedOn)
                });

                // 产出数量（重复尾工序出站时，要注意数量扣减）
                updateFinishQtyCommands.Add(new UpdateQtyByWorkOrderIdCommand
                {
                    WorkOrderId = item.Key,
                    Qty = item.Value.Count(w => w.AfterOperationStatus == SfcStatusEnum.Complete && w.Remark == "LastProcedureOutStation"),
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
