using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 合格代码类型枚举
    /// </summary>
    public enum RejudgeUnqualifiedCodeEnum : sbyte
    {
        /// <summary>
        /// 标记缺陷编码
        /// </summary>
        [Description("标记缺陷编码")]
        Mark = 1,
        /// <summary>
        /// 最终缺陷编码
        /// </summary>
        [Description("最终缺陷编码")]
        Last = 2,
        /// <summary>
        /// 阻断缺陷编码
        /// </summary>
        [Description("阻断缺陷编码")]
        Block = 3
    }
}
