using Hymson.MES.BackgroundServices.Stator;

namespace Hymson.MES.Data.Repositories.Stator
{
    /// <summary>
    /// 仓储接口（铜线条码表）
    /// </summary>
    public interface IWireBarCodeRepository
    {
        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<WireBarCodeEntity> entities);

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<WireBarCodeEntity> entities);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WireBarCodeEntity> GetByIdAsync(long id);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<WireBarCodeEntity>> GetEntitiesAsync(WireBarCodeQuery query);

    }
}
