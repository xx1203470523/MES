using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Equipment
{
    /// <summary>
    /// 班次枚举
    /// </summary>
    public enum EquipmentShiftEnum : sbyte
    {
        /// <summary>
        /// 白班
        /// </summary>
        [Description("白班")]
        DayShift = 1,

        /// <summary>
        /// 夜班
        /// </summary>
        [Description("夜班")]
        NightShift = 2
    }
}