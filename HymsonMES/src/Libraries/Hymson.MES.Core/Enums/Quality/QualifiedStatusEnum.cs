using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 合格状态
    /// </summary>
    public enum QualifiedStatusEnum : sbyte
    {
        /// <summary>
        /// 合格
        /// </summary>
        [Description("合格")]
        Qualified = 1,
        /// <summary>
        /// 不合格
        /// </summary>
        [Description("不合格")]
        Unqualified = 0
    }
}
