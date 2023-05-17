using Hymson.MES.Core.Domain.Equipment;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 设备状态仓储接口
    /// </summary>
    public interface IEquipmentStatusRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquipmentStatusEntity entity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(IEnumerable<EquipmentStatusEntity> entities);

        /// <summary>
        /// 新增（统计）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertStatisticsAsync(EquipmentStatusStatisticsEntity entity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquipmentStatusEntity entity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(IEnumerable<EquipmentStatusEntity> entities);

        /// <summary>
        /// 根据设备ID获取最新的状态记录
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        Task<EquipmentStatusEntity> GetLastEntityByEquipmentIdAsync(long equipmentId);

    }
}
