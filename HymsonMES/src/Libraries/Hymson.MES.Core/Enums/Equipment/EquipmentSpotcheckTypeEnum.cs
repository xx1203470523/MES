using System;
using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 设备点检类型
    /// </summary>
    public enum EquipmentSpotcheckTypeEnum : sbyte
    {
        /// <summary>
        /// 日
        /// </summary>
        [Description("日")]
        Day = 2,
        /// <summary>
        /// 周
        /// </summary>
        [Description("周")]
        Week = 3

    }


    /// <summary>
    /// 设备保养类型
    /// </summary>
    public enum EquipmentMaintenanceTypeEnum : sbyte
    {
        /// <summary>
        /// 日
        /// </summary>
        [Description("日")]
        Day = 2,
        /// <summary>
        /// 周
        /// </summary>
        [Description("周")]
        Week = 3
    }

    /// <summary>
    /// 周期类型
    /// </summary>
    public enum EquipmentCycleTypeEnum : sbyte
    {
        /// <summary>
        /// 小时
        /// </summary>
        [Description("小时")]
        Hour = 2,
        /// <summary>
        /// 日
        /// </summary>
        [Description("天")]
        Day = 3
    }
}
