using Hymson.MES.Core.Domain.Equipment;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 设备状态仓储接口
    /// </summary>
    public interface IEquStatusRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquStatusEntity entity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(IEnumerable<EquStatusEntity> entities);

        /// <summary>
        /// 新增（统计）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertStatisticsAsync(EquStatusStatisticsEntity entity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquStatusEntity entity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(IEnumerable<EquStatusEntity> entities);

        /// <summary>
        /// 根据设备ID获取最新的状态记录
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        Task<EquStatusEntity> GetLastEntityByEquipmentIdAsync(long equipmentId);

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="equStatusQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<EquStatusStatisticsEntity>> GetEquStatusStatisticsEntitiesAsync(EquStatusStatisticsQuery equStatusQuery);
    }
}
