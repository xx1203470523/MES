/*
 *creator: Karl
 *
 *describe: 工单激活（物理删除）    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  Karl
 *build datetime: 2023-04-12 11:14:23
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Plan
{
    /// <summary>
    /// 工单激活（物理删除），数据实体对象   
    /// plan_work_order_bind
    /// @author Karl
    /// @date 2023-04-12 11:14:23
    /// </summary>
    public class PlanWorkOrderBindEntity : BaseEntity
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public long? EquipmentId { get; set; }

       /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }

       /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
