using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Integrated
{
    /// <summary>
    /// 日历类型枚举
    /// </summary>
    public enum MaterialMethodEnum : sbyte
    {
        /// <summary>
        /// 直接领料
        /// </summary>
        [Description("直接领料")]
        Picking = 1,
        /// <summary>
        /// 直接倒冲
        /// </summary>
        [Description("直接倒冲")]
        Backflush = 2,
        /// <summary>
        /// 调拨领料
        /// </summary>
        [Description("调拨领料")]
        TransferPicking = 3,
        /// <summary>
        /// 调拨倒中
        /// </summary>
        [Description("调拨倒冲")]
        TransferBackflush = 4,
        /// <summary>
        /// 不发料
        /// </summary>
        [Description("不发料")]
        NotPicking = 7
    }
    
}
