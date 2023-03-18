using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Integrated
{
    /// <summary>
    /// 编码规则-重置序号
    /// </summary>
    public enum CodeRuleResetTypeEnum : sbyte
    {
        /// <summary>
        /// 从不
        /// </summary>
        [Description("从不")]
        None = 1,
        /// <summary>
        /// 每天
        /// </summary>
        [Description("每天")]
        EveryDay = 2,
        /// <summary>
        /// 每周
        /// </summary>
        [Description("每周")]
        Weekly = 3,
        /// <summary>
        /// 每月
        /// </summary>
        [Description("每月")]
        Monthly = 4,
        /// <summary>
        /// 每年
        /// </summary>
        [Description("每年")]
        Annually = 5
    }
}
