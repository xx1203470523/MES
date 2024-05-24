using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 设备类型（TODO 随便写的类型）
    /// </summary>
    public enum EquipmentOperationPermissionsTypeEnum : sbyte
    {
        /// <summary>
        /// 点检
        /// </summary>
        [Description("点检")]
        Parts = 1,
        /// <summary>
        /// 保养
        /// </summary>
        [Description("保养")]
        Dress = 2,
        /// <summary>
        /// 维修
        /// </summary>
        [Description("维修")]
        Portable = 3
    }
}
