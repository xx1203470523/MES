using Hymson.MES.Core.Domain.Equipment;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 设备心跳仓储接口
    /// </summary>
    public interface IEquHeartbeatRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equipmentHeartbeatEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquHeartbeatEntity equipmentHeartbeatEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equipmentHeartbeatEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(IEnumerable<EquHeartbeatEntity> equipmentHeartbeatEntitys);


        /// <summary>
        /// 新增（记录）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertRecordAsync(EquHeartbeatRecordEntity entity);

        /// <summary>
        /// 批量新增（记录）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRecordsAsync(IEnumerable<EquHeartbeatRecordEntity> entities);

    }
}
