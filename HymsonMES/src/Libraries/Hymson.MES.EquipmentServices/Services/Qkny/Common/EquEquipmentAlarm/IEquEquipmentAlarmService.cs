using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.EquEquipmentAlarm;

namespace Hymson.MES.Services.Services.EquEquipmentAlarm
{
    /// <summary>
    /// 服务接口（设备报警记录）
    /// </summary>
    public interface IEquEquipmentAlarmService
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> AddAsync(EquEquipmentAlarmSaveDto saveDto);
    }
}