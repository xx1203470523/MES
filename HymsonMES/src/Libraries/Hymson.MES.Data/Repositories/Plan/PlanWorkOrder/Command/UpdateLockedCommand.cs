using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command
{
    /// <summary>
    /// 修改工单数量（投入/完工）
    /// </summary>
    public class UpdateLockedCommand : UpdateCommand
    {
        /// <summary>
        /// 工单id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 工单状态
        /// </summary>
        public PlanWorkOrderStatusEnum Status { get; set; }

        /// <summary>
        /// 锁定状态时锁定前的状态
        /// </summary>
        public PlanWorkOrderStatusEnum? LockedStatus { get; set; }
    }
}
