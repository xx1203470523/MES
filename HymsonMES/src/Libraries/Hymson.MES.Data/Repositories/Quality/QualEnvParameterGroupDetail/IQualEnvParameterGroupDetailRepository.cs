using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Quality.Query;

namespace Hymson.MES.Data.Repositories.Quality
{
    /// <summary>
    /// 仓储接口（环境检验参数项目表）
    /// </summary>
    public interface IQualEnvParameterGroupDetailRepository
    {
        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<QualEnvParameterGroupDetailEntity> entities);

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
        Task<IEnumerable<QualEnvParameterGroupDetailEntity>> GetEntitiesAsync(QualEnvParameterGroupDetailQuery query);
        
    }
}
