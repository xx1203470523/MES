using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 启用状态枚举
    /// </summary>
    public enum EnableEnum : sbyte
    {
        /// <summary>
        /// 未启用
        /// </summary>
        [Description("未启用")]
        No = 0,
        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")]
        Yes = 1,
    }
}
