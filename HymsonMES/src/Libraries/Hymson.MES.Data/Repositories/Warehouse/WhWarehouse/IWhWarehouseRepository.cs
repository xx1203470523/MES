using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.WhWareHouse;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.WhWareHouse.Query;

namespace Hymson.MES.Data.Repositories.WhWareHouse
{
    /// <summary>
    /// 仓储接口（仓库）
    /// </summary>
    public interface IWhWarehouseRepository
    {
        #region 新增

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(WhWarehouseEntity entity);

        /// <summary>
        /// 新增忽略重复
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertIgnoreAsync(WhWarehouseEntity entity);

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<WhWarehouseEntity> entities);

        #endregion


        #region 更新

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(WhWarehouseEntity entity);
        
        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<WhWarehouseEntity> entities);

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

        #endregion

        #region 查询

        ///// <summary>
        ///// 根据ID获取数据
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //Task<WhWarehouseEntity> GetByIdAsync(long id);

        /// <summary>
        /// 获取单条
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<WhWarehouseEntity> GetOneAsync(WhWarehouseQuery query);

        ///// <summary>
        ///// 根据IDs获取数据（批量）
        ///// </summary>
        ///// <param name="ids"></param>
        ///// <returns></returns>
        //Task<IEnumerable<WhWarehouseEntity>> GetByIdsAsync(IEnumerable<long> ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<WhWarehouseEntity>> GetEntitiesAsync(WhWarehouseQuery query);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<WhWarehouseEntity>> GetPagedListAsync(WhWarehousePagedQuery pagedQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<WhWarehouseEntity>> GetPagedListCopyAsync(WhWarehousePagedQuery pagedQuery);

        #endregion

    }
}
