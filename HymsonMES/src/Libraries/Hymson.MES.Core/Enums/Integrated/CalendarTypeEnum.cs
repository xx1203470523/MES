using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Integrated
{
    /// <summary>
    /// 日历类型枚举
    /// </summary>
    public enum CalendarTypeEnum : sbyte
    {
        /// <summary>
        /// 设备
        /// </summary>
        [Description("设备")]
        Equipment = 1,
        /// <summary>
        /// 线体
        /// </summary>
        [Description("线体")]
        WorkCenter = 2
    }
    
}
