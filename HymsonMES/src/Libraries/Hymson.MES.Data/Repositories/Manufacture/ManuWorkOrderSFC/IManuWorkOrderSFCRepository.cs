using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.Query;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 仓储接口（工单条码记录表）
    /// </summary>
    public interface IManuWorkOrderSFCRepository
    {
        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> IgnoreRangeAsync(IEnumerable<ManuWorkOrderSFCEntity> entities);

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> RepalceRangeAsync(IEnumerable<ManuWorkOrderSFCEntity> entities);

        /// <summary>
        /// 删除（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> DeleteRangeAsync(IEnumerable<ManuWorkOrderSFCEntity> entities);

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuWorkOrderSFCEntity>> GetEntitiesAsync(EntityByWorkOrderIdQuery query);

    }
}
