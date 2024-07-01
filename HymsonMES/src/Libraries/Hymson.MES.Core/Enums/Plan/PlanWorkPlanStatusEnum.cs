using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 生产计划状态 1：待派发；2：已派发；3：已取消；
    /// </summary>
    public enum PlanWorkPlanStatusEnum : sbyte
    {
        /// <summary>
        /// 未开始
        /// </summary>
        [Description("未开始")]
        NotStarted = 1,
        /// <summary>
        /// 已派发
        /// </summary>
        [Description("已派发")]
        Distributed = 2,
        /// <summary>
        /// 已取消
        /// </summary>
        [Description("已取消")]
        Canceled = 3
    }
}
