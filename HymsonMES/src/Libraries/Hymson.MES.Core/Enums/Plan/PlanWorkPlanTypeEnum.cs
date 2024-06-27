using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Plan
{
    /// <summary>
    /// 生产计划类型
    /// </summary>
    public enum PlanWorkPlanTypeEnum : sbyte
    {
        /// <summary>
        /// 转子 
        /// </summary> 
        [Description("转子")]
        Rotor = 1,
        /// <summary>
        /// 定子
        /// </summary>
        [Description("定子")]
        Stator = 2
    }

}
