using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process.Query;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 仓储接口（产品检验参数项目表）
    /// </summary>
    public interface IProcProductParameterGroupDetailRepository
    {
        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<ProcProductParameterGroupDetailEntity> entities);

        /// <summary>
        /// 删除（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeleteByParentIdAsync(DeleteByParentIdCommand command);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProductParameterGroupDetailEntity>> GetEntitiesAsync(ProcProductParameterGroupDetailQuery query);
        
    }
}
