using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Plan
{
    /// <summary>
    /// 数据实体对象（生产计划）
    /// @author Czhipu
    /// @date 2024-06-16
    /// </summary>
    public partial class PlanWorkPlanEntity : BaseEntity
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 计划单号
        /// </summary>
        public string WorkPlanCode { get; set; }

        /// <summary>
        /// 需求单号
        /// </summary>
        public string? RequirementNumber { get; set; } = "";

        /// <summary>
        /// 计划类型
        /// </summary>
        public PlanWorkOrderTypeEnum Type { get; set; }

        /// <summary>
        /// 计划状态
        /// </summary>
        public PlanWorkPlanStatusEnum Status { get; set; }

        /// <summary>
        /// 超生产比例;默认是0，若允许超产，则写超产的%比例
        /// </summary>
        public decimal OverScale { get; set; } = 0;

        /// <summary>
        /// 开始时间（计划）
        /// </summary>
        public DateTime PlanStartTime { get; set; }

        /// <summary>
        /// 结束时间（计划）
        /// </summary>
        public DateTime PlanEndTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
