using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.NIO;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.NIO.NioPushKeySubordinate.View;
using Hymson.MES.Data.Repositories.NIO.Query;

namespace Hymson.MES.Data.Repositories.NIO
{
    /// <summary>
    /// 仓储接口（物料及其关键下级件信息表）
    /// </summary>
    public interface INioPushKeySubordinateRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(NioPushKeySubordinateEntity entity);
        
        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<NioPushKeySubordinateEntity> entities);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(NioPushKeySubordinateEntity entity);
        
        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<NioPushKeySubordinateEntity> entities);

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
        Task<NioPushKeySubordinateEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<NioPushKeySubordinateEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 根据niopushID获取数据
        /// </summary>
        /// <param name="nioPushId"></param>
        /// <returns></returns>
        Task<IEnumerable<NioPushKeySubordinateEntity>> GetByPushIdAsync(long nioPushId);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<NioPushKeySubordinateEntity>> GetEntitiesAsync(NioPushKeySubordinateQuery query);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<NioPushKeySubordinateView>> GetPagedListAsync(NioPushKeySubordinatePagedQuery pagedQuery);

    }
}
