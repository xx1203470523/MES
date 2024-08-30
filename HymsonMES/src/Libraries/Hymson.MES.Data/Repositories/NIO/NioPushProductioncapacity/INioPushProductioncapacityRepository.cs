using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.NIO;
using Hymson.MES.Core.Domain.NioPushCollection;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.NIO.NioPushProductionCapacity.Query.View;
using Hymson.MES.Data.Repositories.NIO.Query;

namespace Hymson.MES.Data.Repositories.NIO
{
    /// <summary>
    /// 仓储接口（合作伙伴精益与生产能力）
    /// </summary>
    public interface INioPushProductioncapacityRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(NioPushProductioncapacityEntity entity);
        
        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<NioPushProductioncapacityEntity> entities);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(NioPushProductioncapacityEntity entity);
        
        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<NioPushProductioncapacityEntity> entities);

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
        Task<NioPushProductioncapacityEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<NioPushProductioncapacityEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<NioPushProductioncapacityEntity>> GetEntitiesAsync(NioPushProductioncapacityQuery query);

        /// <summary>
        /// 根据niopushID获取数据
        /// </summary>
        /// <param name="nioPushId"></param>
        /// <returns></returns>
        Task<IEnumerable<NioPushProductioncapacityEntity>> GetByPushIdAsync(long nioPushId);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<NioPushProductioncapacityView>> GetPagedListAsync(NioPushProductioncapacityPagedQuery pagedQuery);

    }
}
