using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Plan.Query
{
    /// <summary>
    /// 生产计划信息表 分页参数
    /// </summary>
    public class PlanWorkPlanPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string? OrderCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 工作中心代码
        /// </summary>
        public string? WorkCenterCode { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanWorkOrderTypeEnum? Type { get; set; }

        /// <summary>
        /// 工单状态;1：未开始；2：下达；3：生产中；4：完成；5：锁定；6：暂停中；
        /// </summary>
        public PlanWorkOrderStatusEnum? Status { get; set; }

        /// <summary>
        /// 计划开始时间  时间范围  数组
        /// </summary>
        public DateTime[]? PlanStartTime { get; set; }

        /// <summary>
        /// 查询状态集合
        /// </summary>
        public IEnumerable<PlanWorkOrderStatusEnum>? Statuss { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string? MaterialVersion { get; set; }

        /// <summary>
        /// 产线id
        /// </summary>
        public IEnumerable<long>? WorkCenterIds { get; set; }

        /// <summary>
        /// 不包含的id组
        /// </summary>
        public IEnumerable<long>? NotInIds { get; set; }

        /// <summary>
        /// 工艺路线组
        /// </summary>
        public IEnumerable<long>? ProcessRouteIds { get; set; }
    }

    /// <summary>
    /// 生产计划信息表 分页参数
    /// </summary>
    public class PlanWorkPlanProductPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string? ProductCode { get; set; }

        /// <summary>
        /// ID集合（生产计划）
        /// </summary>
        public IEnumerable<long>? WorkPlanIds { get; set; }

    }

}
