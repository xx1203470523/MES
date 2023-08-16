using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Plan
{
    /// <summary>
    /// 工单激活，数据实体对象   
    /// plan_work_order_activation
    /// @author Czhipu
    /// @date 2023-03-29 09:28:30
    /// </summary>
    public class PlanWorkOrderActivationEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

       /// <summary>
        /// 线体Id
        /// </summary>
        public long LineId { get; set; }

       
    }
}
