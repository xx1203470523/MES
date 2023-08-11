using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.InteEvent.Command;
using Hymson.MES.Data.Repositories.Integrated.InteEvent.View;
using Hymson.MES.Data.Repositories.Integrated.Query;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 仓储接口（事件维护）
    /// </summary>
    public interface IInteEventRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(InteEventEntity entity);

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<InteEventEntity> entities);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(InteEventEntity entity);

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<InteEventEntity> entities);

        /// <summary>
        /// 批量修改事件的事件类型
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdateEventTypeIdAsync(UpdateEventTypeIdCommand command);

        /// <summary>
        /// 清空事件的事件类型
        /// </summary>
        /// <param name="eventTypeId"></param>
        /// <returns></returns>
        Task<int> ClearEventTypeIdAsync(long eventTypeId);

        /// <summary>
        /// 清空事件的事件类型
        /// </summary>
        /// <param name="eventTypeId"></param>
        /// <returns></returns>
        Task<int> ClearEventTypeIdsAsync(long[] eventTypeIds);

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
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<InteEventEntity> GetByCodeAsync(EntityByCodeQuery query);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteEventEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<InteEventEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<InteEventEntity>> GetEntitiesAsync(EntityBySiteIdQuery query);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<InteEventView>> GetPagedListAsync(InteEventPagedQuery pagedQuery);

    }
}
