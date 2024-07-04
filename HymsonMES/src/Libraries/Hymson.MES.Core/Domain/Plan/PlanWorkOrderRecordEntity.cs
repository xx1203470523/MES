/*
 *creator: Karl
 *
 *describe: 工单统计表    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  wangkeming
 *build datetime: 2023-04-19 11:22:22
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Plan
{
    /// <summary>
    /// 工单统计表，数据实体对象   
    /// plan_work_order_record
    /// @author wangkeming
    /// @date 2023-04-19 11:22:22
    /// </summary>
    public class PlanWorkOrderRecordEntity : BaseEntity
    {
        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? RealStart { get; set; }

       /// <summary>
        /// 实际结束时间
        /// </summary>
        public DateTime? RealEnd { get; set; }

       /// <summary>
        /// 投入数量
        /// </summary>
        public decimal? InputQty { get; set; }

       /// <summary>
        /// 报废数量
        /// </summary>
        public decimal? UnqualifiedQuantity { get; set; }

       /// <summary>
        /// 完工数量
        /// </summary>
        public decimal? FinishProductQuantity { get; set; }

       /// <summary>
        /// 下达数量
        /// </summary>
        public decimal? PassDownQuantity { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 工单Id
        /// </summary>
        public long WorkOrderId { get; set; }
        /// <summary>
        /// 工单计划Id
        /// </summary>
        public long? WorkPlanId { get; set; }


    }
}
