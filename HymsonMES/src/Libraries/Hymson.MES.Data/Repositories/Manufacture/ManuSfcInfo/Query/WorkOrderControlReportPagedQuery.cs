/*
 *creator: Karl
 *
 *describe: 工单报告 分页查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-21 04:00:29
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfcInfo.Query
{
    /// <summary>
    /// 工单报告 分页参数
    /// </summary>
    public class WorkOrderControlReportPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string? WorkOrderCode { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanWorkOrderTypeEnum? Type { get; set; }

        /// <summary>
        /// 工单状态
        /// </summary>
        public PlanWorkOrderStatusEnum? Status { get; set; }

        /// <summary>
        /// 录入时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
    }

    /// <summary>
    /// 工单报告 分页参数
    /// </summary>
    public class WorkOrderControlReportOptimizePagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string? WorkOrderCode { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanWorkOrderTypeEnum? Type { get; set; }

        /// <summary>
        /// 工单状态
        /// </summary>
        public PlanWorkOrderStatusEnum? Status { get; set; }

        /// <summary>
        /// 录入时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
    }

    /// <summary>
    /// 获取工单数量
    /// </summary>
    public class WorkOrderQtyQuery
    {
        /// <summary>
        /// 工单数量
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工单ID列表
        /// </summary>
        public List<long> OrderIdList { get; set; }
    }

}
