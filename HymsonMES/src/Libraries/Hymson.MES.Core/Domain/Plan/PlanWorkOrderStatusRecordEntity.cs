/*
 *creator: Karl
 *
 *describe: 工单变更改记录表    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  Karl
 *build datetime: 2023-03-30 03:46:15
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
    /// 工单变更改记录表，数据实体对象   
    /// plan_work_order_status_record
    /// @author Karl
    /// @date 2023-03-30 03:46:15
    /// </summary>
    public class PlanWorkOrderStatusRecordEntity : BaseEntity
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public long? ProductId { get; set; }

        /// <summary>
        /// 工作中心
        /// </summary>
        public long? WorkCenterId { get; set; }

        /// <summary>
        /// 工艺路线
        /// </summary>
        public long? ProcessRouteId { get; set; }

        /// <summary>
        /// 产品bom
        /// </summary>
        public long? ProductBOMId { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanWorkOrderTypeEnum Type { get; set; }

        /// <summary>
        /// 工单状态
        /// </summary>
        public PlanWorkOrderStatusEnum Status { get; set; }

        /// <summary>
        /// 工单数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 超产比例
        /// </summary>
        public decimal OverScale { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime PlanStartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime PlanEndTime { get; set; }

        /// <summary>
        /// 是否锁定;1：是；2：否；
        /// </summary>
        public YesOrNoEnum? IsLocked { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
    }
}
