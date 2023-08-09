using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 升级等级枚举
    /// </summary>
    public enum UpgradeLevelEnum : sbyte
    {
        /// <summary>
        /// 第一等级
        /// </summary>
        [Description("第一等级")]
        One = 1,
        /// <summary>
        /// 第二等级
        /// </summary>
        [Description("第二等级")]
        Two = 2,
        /// <summary>
        /// 第三等级
        /// </summary>
        [Description("第三等级")]
        Three = 3
    }
}
