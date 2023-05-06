using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command
{
    /// <summary>
    /// 更新生产订单记录的实际结束时间
    /// </summary>
    public class UpdateWorkOrderRealTimeCommand : UpdateCommand
    {
        /// <summary>
        /// 生产订单ID
        /// </summary>
        public long[] WorkOrderIds { get; set; }

    }
}
