using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Integrated
{
    /// <summary>
    /// 编码规则-包装等级
    /// </summary>
    public enum CodeRulePackTypeEnum : sbyte
    {
        /// <summary>
        /// 一级
        /// </summary>
        [Description("一级")]
        OneLevel = 1,
        /// <summary>
        /// 二级
        /// </summary>
        [Description("二级")]
        TwoLevel = 2,
        /// <summary>
        /// 三级
        /// </summary>
        [Description("三级")]
        ThreeLevel = 3
    }
}
