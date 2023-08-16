using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 设备类型（TODO 随便写的类型）
    /// </summary>
    public enum EquipmentTypeEnum : sbyte
    {
        /// <summary>
        /// 配件
        /// </summary>
        [Description("配件")]
        Parts = 1,
        /// <summary>
        /// 穿戴设备
        /// </summary>
        [Description("穿戴设备")]
        Dress = 2,
        /// <summary>
        /// 便携设备
        /// </summary>
        [Description("便携设备")]
        Portable = 3
    }
}
