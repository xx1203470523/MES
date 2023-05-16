using Hymson.MES.Core.Domain.Equipment;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 设备报警信息仓储接口
    /// </summary>
    public interface IEquipmentAlarmRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equipmentAlarmEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquipmentAlarmEntity equipmentAlarmEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equipmentAlarmEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<EquipmentAlarmEntity> equipmentAlarmEntitys);

    }
}
