using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.ManuJzBind;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment.Qkny.ManuJzBind.Command;
using Hymson.MES.Data.Repositories.ManuJzBind.Query;

namespace Hymson.MES.Data.Repositories.ManuJzBind
{
    /// <summary>
    /// 仓储接口（极组绑定）
    /// </summary>
    public interface IManuJzBindRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuJzBindEntity entity);

        /// <summary>
        /// 根据极组条码查询信息
        /// </summary>
        /// <param name="jzSfc"></param>
        /// <returns></returns>
        Task<ManuJzBindEntity> GetByJzSfcAsync(ManuJzBindQuery query);

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeletePhysicsAsync(long id);

        /// <summary>
        /// 根据id更新电芯码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> UpdateSfcById(UpdateSfcByIdCommand command);

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<ManuJzBindEntity> entities);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuJzBindEntity entity);
        
        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<ManuJzBindEntity> entities);

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
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuJzBindEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuJzBindEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuJzBindEntity>> GetEntitiesAsync(ManuJzBindQuery query);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuJzBindEntity>> GetPagedListAsync(ManuJzBindPagedQuery pagedQuery);

    }
}
