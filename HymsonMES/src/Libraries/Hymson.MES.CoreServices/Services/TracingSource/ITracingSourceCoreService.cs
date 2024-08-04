using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSFCNode.View;

namespace Hymson.MES.CoreServices.Services
{
    /// <summary>
    /// 条码追溯服务接口
    /// </summary>
    public interface ITracingSourceCoreService
    {
        /// <summary>
        /// 条码追溯（反向）  原始数据 平铺数据 没经过加工的树
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSFCNodeEntity>> OriginalSourceAsync(EntityBySFCQuery query);

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

        /// <summary>
        /// 条码追溯（正向） 平铺列表数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSFCNodeView>> DestinationListAsync(EntityBySFCQuery query);

    }
}