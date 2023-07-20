using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Integrated
{
    /// <summary>
    /// 编码规则-编码模式
    /// </summary>
    public enum CodeRuleCodeModeEnum : sbyte
    {
        /// <summary>
        /// 单个
        /// </summary>
        [Description("单个")]
        One = 1,
        /// <summary>
        /// 多个
        /// </summary>
        [Description("多个")]
        More = 2
    }
}
