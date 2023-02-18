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
        None = 0,
        /// <summary>
        /// 备件
        /// </summary>
        SparePart = 1,
        /// <summary>
        /// 工装
        /// </summary>
        Consumable = 2
    }
}
