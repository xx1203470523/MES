namespace Hymson.MES.EquipmentServices.Request.Equipment
{
    /// <summary>
    /// 请求参数（设备心跳）
    /// </summary>
    public class EquipmentHeartbeatRequest: BaseRequest
    {
        /// <summary>
        /// 设备是否在线（皆上报为true PC上位机判断与PLC断开连接上报false）
        /// </summary>
        public bool IsOnline { get; set; } = false;
    }
}
