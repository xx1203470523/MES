using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.NioPushCollection;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.NIO.NioPushCollection.View;
using Hymson.MES.Data.Repositories.NioPushCollection.Query;

namespace Hymson.MES.Data.Repositories.NioPushCollection
{
    /// <summary>
    /// 仓储接口（NIO推送参数）
    /// </summary>
    public interface INioPushCollectionRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(NioPushCollectionEntity entity);
        
        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<NioPushCollectionEntity> entities);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(NioPushCollectionEntity entity);
        
        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<NioPushCollectionEntity> entities);

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
        Task<NioPushCollectionEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据niopushID获取数据
        /// </summary>
        /// <param name="nioPushId"></param>
        /// <returns></returns>
        Task<IEnumerable<NioPushCollectionEntity>> GetByPushIdAsync(long nioPushId);

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<NioPushCollectionEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<NioPushCollectionEntity>> GetEntitiesAsync(NioPushCollectionQuery query);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<NioPushCollectionStatusView>> GetPagedListAsync(NioPushCollectionPagedQuery pagedQuery);

        /// <summary>
        /// 获取重复List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<NioPushCollectionRepeatView>> GetRepeatEntitiesAsync(NioPushCollectionRepeatQuery query);

        /// <summary>
        /// 获取指定条码+工序List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<NioPushCollectionSfcView>> GetEntitiesBySfcAsync(NioPushCollectionSfcQuery query);
    }
}
