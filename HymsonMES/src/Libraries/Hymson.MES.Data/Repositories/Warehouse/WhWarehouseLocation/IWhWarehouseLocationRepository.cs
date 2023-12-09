using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.WhWarehouseLocation;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.WhWarehouseLocation.Query;

namespace Hymson.MES.Data.Repositories.WhWarehouseLocation
{
    /// <summary>
    /// 仓储接口（库位）
    /// </summary>
    public interface IWhWarehouseLocationRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(WhWarehouseLocationEntity entity);
        
        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<WhWarehouseLocationEntity> entities);

        /// <summary>
        /// 新增Ignore（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertIgnoreRangeAsync(IEnumerable<WhWarehouseLocationEntity> entities);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(WhWarehouseLocationEntity entity);
        
        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<WhWarehouseLocationEntity> entities);

        /// <summary>
        /// 软删除  
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);
        
        /// <summary>
        /// 软删除（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand command);


        /// <summary>
        /// 物理删除（批量）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeletesPhysicsAsync(DeleteCommand command);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WhWarehouseLocationEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<WhWarehouseLocationEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<WhWarehouseLocationEntity>> GetEntitiesAsync(WhWarehouseLocationQuery query);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<WhWarehouseLocationEntity>> GetPagedListAsync(WhWarehouseLocationPagedQuery pagedQuery);

    }
}
