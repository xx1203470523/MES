using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 工单类型
    /// </summary>
    public enum PlanWorkOrderTypeEnum : sbyte
    {
        /// <summary>
        /// 试产工单
        /// </summary>
        [Description("试产工单")]
        TrialProduction = 1,
        /// <summary>
        /// 测试工单
        /// </summary>
        [Description("测试工单")]
        Test = 2,
        /// <summary>
        /// 量产工单
        /// </summary>
        [Description("量产工单")]
        Output = 3,
    }
}
