using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Integrated
{
    /// <summary>
    /// 编码规则-编码类型
    /// </summary>
    public enum CodeRuleCodeTypeEnum : sbyte
    {
        /// <summary>
        /// 过程控制序列码
        /// </summary>
        [Description("过程控制序列码")]
        ProcessControlSeqCode = 1,
        /// <summary>
        /// 包装序列码
        /// </summary>
        [Description("包装序列码")]
        PackagingSeqCode = 2
    }
}
