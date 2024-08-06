using Hymson.MES.Data.Repositories.Common.Query;
using System.Data;

namespace Hymson.MES.BackgroundServices.Stator
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    public interface IOPRepository<TEntity> where TEntity : BaseOPEntity
    {
        /// <summary>
        /// 根据水位批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetListByStartWaterMarkIdAsync(EntityByWaterMarkQuery query);

        /// <summary>
        /// 根据水位批量获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetListByStartWaterMarkIdAsync(EntityByWaterMarkQuery query, string tableName);

        /// <summary>
        /// 根据水位批量获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<DataTable> GetDataTableByStartWaterMarkIdAsync(EntityByWaterMarkQuery query);

    }
}
