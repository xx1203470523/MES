using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture;

namespace Hymson.MES.CoreServices.Services.Manufacture
{
    /// <summary>
    /// 降级品继承
    /// @author Czhipu
    /// @date 2023-08-30
    /// </summary>
    public interface IManuDegradedProductExtendService
    {
        /// <summary>
        /// 创建降级品记录
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        Task<int> CreateManuDowngradingsAsync(DegradedProductExtendBo bo);

        /// <summary>
        /// 批量查询条码对应的降级录入数据
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuDowngradingEntity>> GetManuDownGradingsAsync(DegradedProductExtendBo bo);

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentEntities"></param>
        /// <returns></returns>
        Task<int> CreateManuDowngradingsByConsumesAsync(DegradedProductExtendBo bo, IEnumerable<ManuDowngradingEntity>? downgradingEntities);

    }
}
