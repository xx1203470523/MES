using Hymson.ClearCache;
using Hymson.EventBus.Abstractions;
using Hymson.Infrastructure.Enums;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hymson.MES.BackgroundTasks.HostedServices
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SubHostedService : IHostedService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IEventBus<EventBusInstance1> _eventBus;
        private readonly IClearCacheService _clearCacheService;
        private readonly ILogger<SubHostedService> _logger;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventBus"></param>
        /// <param name="clearCacheService"></param>
        public SubHostedService(IEventBus<EventBusInstance1> eventBus,
            IClearCacheService clearCacheService, ILogger<SubHostedService> logger)
        {
            _eventBus = eventBus;
            _clearCacheService = clearCacheService;
            _logger = logger;
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
                //缓存清理服务
                await _clearCacheService.ClearCacheAsync(new ServiceTypeEnum[] { ServiceTypeEnum.MES }, cancellationToken);
                //分表初始化服务 只初始化一次即可
                //await _deliveryService.InitializeSplitTableAsync(DbName.MES_MASTER, ManuSfcStepRepository.TEMPLATETABLENAME, _options.Value.Divides);
                SubscribeManufactureServices();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "启动出错:");
            }
        }

        /// <summary>
        /// 生产订阅服务
        /// </summary>
        public void SubscribeManufactureServices()
        {

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
