using Hymson.MES.EquipmentServices.Dtos.EquipmentCollect;

namespace Hymson.MES.EquipmentServices.Services.EquipmentCollect
{
    /// <summary>
    /// 设备接口
    /// </summary>
    public interface IEquipmentCollectService
    {
        /// <summary>
        /// 设备心跳
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task EquipmentHeartbeatAsync(EquipmentHeartbeatDto request);

        /// <summary>
        /// 设备状态监控
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task EquipmentStateAsync(EquipmentStateDto request);

        /// <summary>
        /// 设备报警
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task EquipmentAlarmAsync(EquipmentAlarmDto request);

        /// <summary>
        /// 设备停机原因
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task EquipmentDownReasonAsync(EquipmentDownReasonDto request);



        /// <summary>
        /// 设备过程参数采集(无在制品条码)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task EquipmentProcessParamAsync(EquipmentProcessParamDto request);

        /// <summary>
        /// 设备产品过程参数采集(无在制品条码)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task EquipmentProductProcessParamInNotCanSFCAsync(EquipmentProductProcessParamInNotCanSFCDto request);

        /// <summary>
        /// 设备产品过程参数采集
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task EquipmentProductProcessParamAsync(EquipmentProductProcessParamDto request);



    }
}
