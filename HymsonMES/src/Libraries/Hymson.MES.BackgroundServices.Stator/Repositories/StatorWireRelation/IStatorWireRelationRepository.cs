using Hymson.MES.BackgroundServices.Stator;

namespace Hymson.MES.Data.Repositories.Stator
{
    /// <summary>
    /// 仓储接口（定子铜线关系表）
    /// </summary>
    public interface IStatorWireRelationRepository
    {
        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<StatorWireRelationEntity> entities);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<StatorWireRelationEntity> GetByIdAsync(long id);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<StatorWireRelationEntity>> GetEntitiesAsync(StatorWireRelationQuery query);

    }
}
