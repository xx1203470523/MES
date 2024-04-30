using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.Query;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 仓储接口（条码追溯表-反向）
    /// </summary>
    public interface IManuSFCNodeSourceRepository
    {
        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<ManuSFCNodeSourceEntity> entities);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(IEnumerable<ManuSFCNodeSourceEntity> entities);

        /// <summary>
        /// 查询树数据的List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSFCNodeSourceEntity>> GetTreeEntitiesAsync(long nodeId);

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSFCNodeSourceEntity>> GetEntitiesAsync(long nodeId);

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSFCNodeSourceEntity>> GetEntitiesAsync(IEnumerable<long> nodeIds);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSFCNodeSourceEntity>> GetEntitiesAsync(ManuSFCNodeSourceQuery query);

    }
}
