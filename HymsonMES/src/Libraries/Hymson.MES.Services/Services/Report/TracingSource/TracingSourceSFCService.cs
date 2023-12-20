using Hymson.Authentication.JwtBearer.Security;
using Hymson.MES.CoreServices.Services;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Services.Dtos.Report;

namespace Hymson.MES.Services.Services
{
    /// <summary>
    /// 条码追溯服务
    /// </summary>
    public class TracingSourceSFCService : ITracingSourceSFCService
    {
        /// <summary>
        /// 当前站点对象
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 仓储接口（条码追溯）
        /// </summary>
        private readonly ITracingSourceCoreService _tracingSourceCoreService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentSite"></param>
        /// <param name="tracingSourceCoreService"></param>
        public TracingSourceSFCService(ICurrentSite currentSite,
            ITracingSourceCoreService tracingSourceCoreService)
        {
            _currentSite = currentSite;
            _tracingSourceCoreService = tracingSourceCoreService;
        }


        /// <summary>
        /// 条码追溯（反向）
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<NodeSourceDto> SourceAsync(string sfc)
        {
            var data = await _tracingSourceCoreService.SourceAsync(new EntityBySFCQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                SFC = sfc
            });

            return data.ToDto<NodeSourceDto>();
        }

        /// <summary>
        /// 条码追溯（正向）
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<NodeSourceDto> DestinationAsync(string sfc)
        {
            var data = await _tracingSourceCoreService.DestinationAsync(new EntityBySFCQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                SFC = sfc
            });

            return data.ToDto<NodeSourceDto>();
        }

    }


}