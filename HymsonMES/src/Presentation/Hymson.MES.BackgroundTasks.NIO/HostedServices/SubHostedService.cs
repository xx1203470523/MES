using Hymson.ClearCache;
using Hymson.Infrastructure.Enums;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hymson.MES.BackgroundTasks.NIO.HostedServices
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SubHostedService : IHostedService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<SubHostedService> _logger;
        private readonly IClearCacheService _clearCacheService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventBus"></param>
        /// <param name="clearCacheService"></param>
        public SubHostedService(ILogger<SubHostedService> logger, IClearCacheService clearCacheService)
        {
            _logger = logger;
            _clearCacheService = clearCacheService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                // 缓存清理服务
                await _clearCacheService.ClearCacheAsync(new ServiceTypeEnum[] { ServiceTypeEnum.MES }, cancellationToken);

                // 分表初始化服务 只初始化一次即可
                //await _deliveryService.InitializeSplitTableAsync(DbName.MES_MASTER, ManuSfcStepRepository.TEMPLATETABLENAME, _options.Value.Divides);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "启动出错:");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

    }
}
