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
        PackagingSeqCode = 2,

        /// <summary>
        /// IQC
        /// </summary>
        [Description("IQC")]
        IQC = 3,

        /// <summary>
        /// OQC
        /// </summary>
        [Description("OQC")]
        OQC = 4,

        /// <summary>
        /// FQC
        /// </summary>
       [Description("FQC")]
        FQC = 5,

        /// <summary>
        /// FQC
        /// </summary>
        [Description("环境检测")]
        Environment = 6
    }
}
