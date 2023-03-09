using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 等级枚举
    /// </summary>
    public enum LevelEnum : sbyte
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
        None = 0,
        /// <summary>
        /// 一级
        /// </summary>
        [Description("一级")]
        One = 1,
        /// <summary>
        /// 二级
        /// </summary>
        [Description("二级")]
        Two = 2,
        /// <summary>
        /// 三级
        /// </summary>
        [Description("三级")]
        Three = 3
    }
}
