/*
 *creator: Karl
 *
 *describe: 工单激活日志（日志中心）    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  Karl
 *build datetime: 2023-04-12 11:17:04
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Plan
{
    /// <summary>
    /// 工单激活日志（日志中心），数据实体对象   
    /// plan_work_order_bind_record
    /// @author Karl
    /// @date 2023-04-12 11:17:04
    /// </summary>
    public class PlanWorkOrderBindRecordEntity : BaseEntity
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public long? EquipmentId { get; set; }

       /// <summary>
        /// 资源Id
        /// </summary>
        public long ResourceId { get; set; }

       /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

       /// <summary>
        /// 激活类型;1：取消激活；2：激活；
        /// </summary>
        public PlanWorkOrderActivateTypeEnum ActivateType { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
