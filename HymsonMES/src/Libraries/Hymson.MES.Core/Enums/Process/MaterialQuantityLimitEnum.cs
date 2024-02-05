using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 物料数据 数量限制 枚举
    /// </summary>
    public enum MaterialQuantityLimitEnum : sbyte
    {
        /// <summary>
        /// 仅1.0
        /// </summary>
        [Description("仅1.0")]
        OnlyOne = 1,
        /// <summary>
        /// 任意数字
        /// </summary>
        [Description("任意数字")]
        AnyNumber = 2,
    }
}
