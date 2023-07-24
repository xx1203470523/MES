using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 禁用/启用状态枚举
    /// </summary>
    public enum DisableOrEnableEnum : sbyte
    {
        /// <summary>
        /// 已禁用
        /// </summary>
        [Description("已禁用")]
        Disable = 0,
        /// <summary>
        /// 已启用
        /// </summary>
        [Description("已启用")]
        Enable = 1,
    }
}
