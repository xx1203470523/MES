using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Common;
using Hymson.MES.Data.Repositories.Common.Query;

namespace Hymson.MES.Data.Repositories.Common
{
    /// <summary>
    /// 仓储接口（message_push）
    /// </summary>
    public interface IMessagePushRepository
    {
        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MessagePushEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<MessagePushEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<MessagePushEntity>> GetEntitiesAsync(MessagePushQuery query);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<MessagePushEntity>> GetPagedListAsync(MessagePushPagedQuery pagedQuery);

    }
}
