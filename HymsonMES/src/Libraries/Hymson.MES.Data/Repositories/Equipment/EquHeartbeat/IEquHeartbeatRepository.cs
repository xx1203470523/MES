using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 设备心跳仓储接口
    /// </summary>
    public interface IEquHeartbeatRepository
    {
        /// <summary>
        /// 获取设备心跳记录
        /// </summary>
        /// <param name="equHeartbeatQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<EquHeartbeatEntity>> GetEquHeartbeatEntitiesAsync(EquHeartbeatQuery equHeartbeatQuery);

        /// <summary>
        /// 根据查询条件获取设备心跳状态报表分页数据
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquHeartbeatReportView>> GetEquHeartbeatReportPageListAsync(EquHeartbeatReportPagedQuery pageQuery);

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

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equHeartbeatEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquHeartbeatEntity equHeartbeatEntity);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="equHeartbeatEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(IEnumerable<EquHeartbeatEntity> equHeartbeatEntitys);

    }
}
