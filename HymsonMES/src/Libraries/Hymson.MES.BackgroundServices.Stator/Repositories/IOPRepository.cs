using Hymson.MES.Data.Repositories.Common.Query;

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

    }
}
