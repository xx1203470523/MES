﻿using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Equipment
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum EquipmentRecordOperationTypeEnum : sbyte
    {
        /// <summary>
        /// 设备注册
        /// </summary>
        [Description("设备注册")]
        Register = 1,

        /// <summary>
        /// 设备点检
        /// </summary>
        [Description("设备点检")]
        SpotCheck = 2,

        /// <summary>
        /// 设备保养
        /// </summary>
        [Description("设备保养")]
        Maintain = 3,

        /// <summary>
        /// 设备维修
        /// </summary>
        [Description("设备维修")]
        Service = 4,

        /// <summary>
        /// 备件绑定
        /// </summary>
        [Description("备件绑定")]
        SparepartsBind = 5,

        /// <summary>
        /// 备件解绑
        /// </summary>
        [Description("备件解绑")]
        SparepartsUnbind = 6
    }
}
