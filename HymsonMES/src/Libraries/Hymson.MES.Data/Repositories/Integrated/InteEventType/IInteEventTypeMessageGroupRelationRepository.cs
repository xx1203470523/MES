using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.Query;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 仓储接口（事件类型关联群组）
    /// </summary>
    public interface IInteEventTypeMessageGroupRelationRepository
    {
        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<InteEventTypeMessageGroupRelationEntity> entities);

        /// <summary>
        /// 删除（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeleteByParentIdAsync(DeleteByParentIdCommand command);

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<InteEventTypeMessageGroupRelationEntity>> GetEntitiesAsync(EntityByParentIdQuery query);

    }
}
