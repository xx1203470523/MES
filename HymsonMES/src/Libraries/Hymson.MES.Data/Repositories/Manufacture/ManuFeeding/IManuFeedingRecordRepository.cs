using Hymson.MES.Core.Domain.Manufacture;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 上卸料记录表仓储接口
    /// </summary>
    public interface IManuFeedingRecordRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuFeedingRecordEntity entity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(IEnumerable<ManuFeedingRecordEntity> entities);

    }
}
