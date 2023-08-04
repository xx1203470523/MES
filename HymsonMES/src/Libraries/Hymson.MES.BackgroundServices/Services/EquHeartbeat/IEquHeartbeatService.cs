using Hymson.MES.BackgroundServices.Dtos.EquHeartbeat;

namespace Hymson.MES.BackgroundServices.Services.EquHeartbeat
{
    public interface IEquHeartbeatService
    {
        /// <summary>
        /// 设备心跳更新
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task EquipmentHeartbeatUpdateAsync(EquipmentHeartbeatUpdateDto request);

        /// <summary>
        /// 删除之前的心跳数据
        /// </summary>
        /// <param name="months"></param>
        /// <returns></returns>
        Task<int> DeleteMonthsBeforeAsync(int months);
    }
}
