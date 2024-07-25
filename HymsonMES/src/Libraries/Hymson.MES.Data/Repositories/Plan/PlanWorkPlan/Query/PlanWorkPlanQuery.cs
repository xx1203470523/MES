using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Plan.Query
{
    /// <summary>
    /// 生产计划信息表 分页参数
    /// </summary>
    public class PlanWorkPlanQuery
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 生产计划ID
        /// </summary>
        public long? WorkPlanId { get; set; }

        /// <summary>
        /// 生产计划产品ID
        /// </summary>
        public long? WorkPlanProductId { get; set; }

        /// <summary>
        /// 编码集合
        /// </summary>
        public IEnumerable<string>? Codes { get; set; }

        /// <summary>
        /// 计划单号
        /// </summary>
        public string? WorkPlanCode { get; set; }

        /// <summary>
        /// 计划类型(1:试产工单;2:生产工单;)
        /// </summary>
        public PlanWorkOrderTypeEnum Type { get; set; }

        /// <summary>
        /// 计划状态
        /// </summary>
        public PlanWorkPlanStatusEnum Status { get; set; }

        /// <summary>
        /// 计划开始时间（时间范围-数组）
        /// </summary>
        public DateTime[]? PlanStartTime { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class PlanWorkPlanByPlanIdQuery
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 计划Id
        /// </summary>
        public long PlanId { get; set; }
        /// <summary>
        /// 产品Id
        /// </summary>
        public long PlanProductId { get; set; }
    }
}
