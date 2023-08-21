using Hymson.MES.Core.Enums;

namespace Hymson.MES.EquipmentServices.Dtos.EquipmentCollect
{
    /// <summary>
    /// 请求参数（设备报警）
    /// </summary>
    public record EquipmentAlarmDto : BaseDto
    {
        /// <summary>
        ///  状态（1-触发，2-恢复 ）
        /// </summary>
        public EquipmentAlarmStatusEnum Status { get; set; } = EquipmentAlarmStatusEnum.Recover;

        /// <summary>
        ///  报警代码
        /// </summary>
        public string AlarmCode { get; set; } = "";

        /// <summary>
        ///  故障详细信息
        /// </summary>
        public string? AlarmMsg { get; set; }
    }
}
