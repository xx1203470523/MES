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
        ///// <summary>
        ///// 测试工单
        ///// </summary>
        //[Description("测试工单")]
        //Test = 2,
        ///// <summary>
        ///// 量产工单
        ///// </summary>
        //[Description("量产工单")]
        //Output = 3,
        /// <summary>
        /// 生产工单
        /// </summary>
        [Description("生产工单")]
        Production = 2,
        /// <summary>
        /// 返工工单
        /// </summary>
        [Description("返工工单")]
        Rework = 3,
        /// <summary>
        /// 实验工单
        /// </summary>
        [Description("实验工单")]
        Experiment = 4
    }
}
