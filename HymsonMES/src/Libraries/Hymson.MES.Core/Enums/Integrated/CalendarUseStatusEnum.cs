using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Integrated
{
    /// <summary>
    /// 日历启用状态
    /// </summary>

    public enum CalendarUseStatusEnum : sbyte
    {
        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")]
        Enable = 1,
        /// <summary>
        /// 未启用
        /// </summary>
        [Description("未启用")]
        NotEnabled = 0
    }
}
