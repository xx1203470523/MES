using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command
{
    /// <summary>
    /// 修改工单数量（投入/完工）
    /// </summary>
    public class UpdateQtyByWorkOrderIdCommand : UpdateCommand
    {
        /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; } = 0;

    }
}
