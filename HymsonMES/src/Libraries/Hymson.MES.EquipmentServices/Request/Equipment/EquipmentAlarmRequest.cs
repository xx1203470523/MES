﻿using Hymson.MES.Core.Enums;

namespace Hymson.MES.EquipmentServices.Request.Equipment
{
    /// <summary>
    /// 请求参数（设备报警）
    /// </summary>
    public class EquipmentAlarmRequest : BaseRequest
    {
        /// <summary>
        ///  状态（0-恢复 1-触发）
        /// </summary>
        public EquipmentAlarmStatusEnum Status { get; set; } = EquipmentAlarmStatusEnum.Recover;

        /// <summary>
        ///  报警代码
        /// </summary>
        public string AlarmCode { get; set; } = "";

        /// <summary>
        ///  故障详细信息
        /// </summary>
        public int? AlarmMsg { get; set; }
    }
}
