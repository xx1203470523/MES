using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query
{
    /// <summary>
    /// 工单信息表 分页参数
    /// </summary>
    public class PlanWorkOrderPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long? SiteId { get; set; }

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
        /// 领料状态 1：未领料；2：已领料；3：部分领料
        /// </summary>
        public PlanWorkOrderPickStatusEnum? PickStatus { get; set; }

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

        /// <summary>
        /// 创建时间  数组 ：时间范围 
        /// </summary>
        public DateTime[]? CreatedOn { get; set; }

        /// <summary>
        /// 工单Id列表
        /// </summary>
        public IEnumerable<long>? WorkOrderIds { get; set; }

        /// <summary>
        /// 线体ID
        /// </summary>
        public long? LineId { get; set; }
    }
}
