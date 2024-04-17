using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.ManuEquipmentStatusTime;

namespace Hymson.MES.Services.Services.ManuEquipmentStatusTime
{
    /// <summary>
    /// 服务接口（设备状态时间）
    /// </summary>
    public interface IManuEquipmentStatusTimeService
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> AddAsync(ManuEquipmentStatusTimeSaveDto saveDto);
    }
}