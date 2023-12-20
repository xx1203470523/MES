using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.Data.Repositories.Common.Query;

namespace Hymson.MES.CoreServices.Services
{
    /// <summary>
    /// 条码追溯服务接口
    /// </summary>
    public interface ITracingSourceCoreService
    {
        /// <summary>
        /// 条码追溯（反向）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<NodeSourceBo> SourceAsync(EntityBySFCQuery query);

        /// <summary>
        /// 条码追溯（正向）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<NodeSourceBo> DestinationAsync(EntityBySFCQuery query);

    }
}