/*
 *creator: Karl
 *
 *describe: 工单激活记录    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  Karl
 *build datetime: 2023-03-30 02:42:18
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Domain.Plan
{
    /// <summary>
    /// 工单激活记录，数据实体对象   
    /// plan_work_order_activation_record
    /// @author Karl
    /// @date 2023-03-30 02:42:18
    /// </summary>
    public class PlanWorkOrderActivationRecordEntity : BaseEntity
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

       /// <summary>
        /// 激活类型;1：取消激活；2：激活；
        /// </summary>
        public PlanWorkOrderActivateTypeEnum ActivateType { get; set; }

       
    }
}
