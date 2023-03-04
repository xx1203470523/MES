using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 备件/工装类型枚举
    /// </summary>
    public enum EquipmentPartTypeEnum : sbyte
    {
        /// <summary>
        /// 未知设备
        /// </summary>
        [Description("未知设备")]
        None = 0,
        /// <summary>
        /// 备件
        /// </summary>
        [Description("备件")]
        SparePart = 1,
        /// <summary>
        /// 工装
        /// </summary>
        [Description("工装")]
        Consumable = 2
    }
}
