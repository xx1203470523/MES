namespace Hymson.MES.EquipmentServices.Dtos.Equipment
{
    /// <summary>
    /// 请求参数（设备心跳）
    /// </summary>
    public record EquipmentHeartbeatDto : BaseDto
    {
        /// <summary>
        /// 设备是否在线（皆上报为true PC上位机判断与PLC断开连接上报false）
        /// </summary>
        public bool IsOnline { get; set; } = false;
    }
}
