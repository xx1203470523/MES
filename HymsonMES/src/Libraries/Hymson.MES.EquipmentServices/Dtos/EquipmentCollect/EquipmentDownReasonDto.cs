using Hymson.MES.Core.Enums;

namespace Hymson.MES.EquipmentServices.Dtos.EquipmentCollect
{
    /// <summary>
    /// 请求参数（设备停机原因）
    /// </summary>
    public record EquipmentDownReasonDto : BaseDto
    {
        /// <summary>
        ///  设备停机原因（1：维护保养;2：吃饭/休息;3：换型;4：设备改造;5：来料不良;6：设备校验;7：首件/点检;8：品质异常;9：缺备件;10：环境异常;11：设备信息不完善;12：故障停机；）
        /// </summary>
        public EquipmentDownReasonCodeEnum DownReasonCode { get; set; } = EquipmentDownReasonCodeEnum.Unknown;

        /// <summary>
        ///  开始时间
        /// </summary>
        public DateTime? BeginTime { get; set; }

        /// <summary>
        ///  结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}
