using Hymson.MES.Core.Enums;

namespace Hymson.MES.EquipmentServices.Dtos.EquipmentCollect
{
    /// <summary>
    /// 请求参数（设备状态监控）
    /// </summary>
    public record EquipmentStateDto : BaseDto
    {
        /// <summary>
        ///  状态代码（0:自动运行;1：待机;2：正常停机;3：故障停机;4：待料;5：满料；）
        /// </summary>
        public EquipmentStateEnum StateCode { get; set; } = EquipmentStateEnum.AutoRun;
    }
}
