using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Equipment
{
    /// <summary>
    /// 操作类型 1、设备注册2、设备点检3，设备保养4，设备维修，5、备件绑定6、备件解绑
    /// </summary>
    public enum EquEquipmentRecordOperationTypeEnum : sbyte
    {
        /// <summary>
        /// 设备注册
        /// </summary>
        [Description("设备注册")]
        Registration = 1,

        /// <summary>
        /// 设备点检
        /// </summary>
        [Description("设备点检")]
        Inspection = 2,

        /// <summary>
        /// 设备保养
        /// </summary>
        [Description("设备保养")]
        Maintenance = 3,

        /// <summary>
        /// 设备维修
        /// </summary>
        [Description("设备维修")]
        Repair = 4,

        /// <summary>
        /// 备件绑定
        /// </summary>
        [Description("备件绑定")]
        SparePartsBinding = 5,

        /// <summary>
        /// 备件解绑
        /// </summary>
        [Description("备件解绑")]
        SparePartsUnbinding = 6,

        /// <summary>
        /// 工具绑定
        /// </summary>
        [Description("工具绑定")]
        ToolBind = 7,

        /// <summary>
        /// 工具解绑
        /// </summary>
        [Description("工具解绑")]
        ToolUnbind = 8
    }
}
