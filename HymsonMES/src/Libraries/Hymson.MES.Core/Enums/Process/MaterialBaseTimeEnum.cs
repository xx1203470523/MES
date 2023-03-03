using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 物料来源枚举
    /// </summary>
    public enum MaterialBaseTimeEnum : sbyte
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 1,
        /// <summary>
        /// 结合
        /// </summary>
        [Description("结合")]
        Combine = 2,
        /// <summary>
        /// 连续
        /// </summary>
        [Description("连续")]
        Continuation = 3
    }
}
