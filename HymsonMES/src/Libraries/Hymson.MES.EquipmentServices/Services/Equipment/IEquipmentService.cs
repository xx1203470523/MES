using Hymson.MES.EquipmentServices.Request.Equipment;

namespace Hymson.MES.EquipmentServices.Services.Equipment
{
    /// <summary>
    /// 设备接口
    /// </summary>
    public interface IEquipmentService
    {
        /// <summary>
        /// 设备心跳
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task EquipmentHeartbeatAsync(EquipmentHeartbeatRequest request);

        /// <summary>
        /// 设备状态监控
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task EquipmentStateAsync(EquipmentStateRequest request);

        /// <summary>
        /// 设备报警
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task EquipmentAlarmAsync(EquipmentAlarmRequest request);

        /// <summary>
        /// 设备停机原因
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task EquipmentDownReasonAsync(EquipmentDownReasonRequest request);
    }
}
