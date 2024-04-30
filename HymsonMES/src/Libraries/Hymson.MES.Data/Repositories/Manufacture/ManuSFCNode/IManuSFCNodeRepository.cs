using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Query;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 仓储接口（条码追溯表）
    /// </summary>
    public interface IManuSFCNodeRepository
    {
        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<ManuSFCNodeEntity> entities);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuSFCNodeEntity entity);

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<ManuSFCNodeEntity> entities);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(IEnumerable<long> ids);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuSFCNodeEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ManuSFCNodeEntity> GetBySFCAsync(EntityBySFCQuery query);

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSFCNodeEntity>> GetByIdsAsync(IEnumerable<long> ids);

    }
}
