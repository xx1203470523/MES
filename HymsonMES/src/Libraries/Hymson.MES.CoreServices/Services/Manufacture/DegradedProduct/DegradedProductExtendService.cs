using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.Data.Repositories.Manufacture;

namespace Hymson.MES.CoreServices.Services.Manufacture
{
    /// <summary>
    /// 降级品继承
    /// @author Czhipu
    /// @date 2023-08-30
    /// </summary>
    public class DegradedProductExtendService : IDegradedProductExtendService
    {
        /// <summary>
        /// 仓储接口（降级录入）
        /// </summary>
        private readonly IManuDowngradingRepository _manuDowngradingRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manuDowngradingRepository"></param>
        public DegradedProductExtendService(IManuDowngradingRepository manuDowngradingRepository)
        {
            _manuDowngradingRepository = manuDowngradingRepository;
        }

        /// <summary>
        /// 批量查询条码对应的降级录入数据
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuDowngradingEntity>> GetManuDownGradingsAsync(MultiSFCBo bo)
        {
            return await _manuDowngradingRepository.GetBySFCsAsync(new ManuDowngradingBySFCsQuery
            {
                SiteId = bo.SiteId,
                SFCs = bo.SFCs
            });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public async Task<int> CreateManuDowngradingsByConsumesAsync(MultiSFCBo bo)
        {
            var currentEntities = await GetManuDownGradingsAsync(bo);
            if (currentEntities == null || currentEntities.Any() == false) return 0;

            /*
            var addEntities = currentEntities.Select(s => new ManuDowngradingEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = s.SiteId,
                SFC = s.CirculationBarCode
            });
            */

            return 0;
        }

    }
}
