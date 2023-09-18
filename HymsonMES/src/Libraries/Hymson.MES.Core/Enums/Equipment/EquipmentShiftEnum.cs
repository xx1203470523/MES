using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Hymson.MES.Core.Enums.Equipment
{
    /// <summary>
    /// 班次枚举
    /// </summary>
    public enum EquipmentShiftEnum : sbyte
    {
        /// <summary>
        /// 白班+夜班
        /// </summary>
        [Description("白班+夜班")]
        WholeShift = 0,

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