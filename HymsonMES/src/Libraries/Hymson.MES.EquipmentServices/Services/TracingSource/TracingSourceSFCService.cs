using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Services;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.Web.Framework.WorkContext;

namespace Hymson.MES.EquipmentServices.Services
{
    /// <summary>
    /// 条码追溯服务
    /// </summary>
    public class TracingSourceSFCService : ITracingSourceSFCService
    {
        /// <summary>
        /// 当前设备对象
        /// </summary>
        private readonly ICurrentEquipment _currentEquipment;

        /// <summary>
        /// 仓储接口（条码追溯）
        /// </summary>
        private readonly ITracingSourceCoreService _tracingSourceCoreService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentEquipment"></param>
        /// <param name="tracingSourceCoreService"></param>
        public TracingSourceSFCService(ICurrentEquipment currentEquipment,
            ITracingSourceCoreService tracingSourceCoreService)
        {
            _currentEquipment = currentEquipment;
            _tracingSourceCoreService = tracingSourceCoreService;
        }


        /// <summary>
        /// 条码追溯（反向）
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<NodeSourceBo> SourceAsync(string sfc)
        {
            return await _tracingSourceCoreService.SourceAsync(new EntityBySFCQuery
            {
                SiteId = _currentEquipment.SiteId,
                SFC = sfc
            });
        }

        /// <summary>
        /// 条码追溯（正向）
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<NodeSourceBo> DestinationAsync(string sfc)
        {
            return await _tracingSourceCoreService.DestinationAsync(new EntityBySFCQuery
            {
                SiteId = _currentEquipment.SiteId,
                SFC = sfc
            });
        }

    }


}