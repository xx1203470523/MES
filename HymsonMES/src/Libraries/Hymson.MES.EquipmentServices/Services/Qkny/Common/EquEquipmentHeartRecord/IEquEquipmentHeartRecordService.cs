using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.EquEquipmentHeartRecord;

namespace Hymson.MES.Services.Services.EquEquipmentHeartRecord
{
    /// <summary>
    /// 服务接口（设备心跳登录记录）
    /// </summary>
    public interface IEquEquipmentHeartRecordService
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> AddAsync(EquEquipmentHeartRecordSaveDto saveDto);
    }
}