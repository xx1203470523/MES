using Hymson.ClearCache;
using Hymson.EventBus.Abstractions;
using Hymson.Infrastructure.Enums;
using Hymson.MES.BackgroundServices.EventHandling;
using Hymson.MES.BackgroundServices.EventHandling.ManufactureHandling.ManuSfcStepHandling;
using Hymson.MES.BackgroundServices.EventHandling.ProcessEventHandling.PrintEventHandling;
using Hymson.MES.BackgroundServices.Quality.FQCOrderAutoCreateIntegration;
using Hymson.MES.Core.Constants;
using Hymson.MES.CoreServices.Events.ManufactureEvents.ManuSfcStepEvents;
using Hymson.MES.CoreServices.Events.ProcessEvents.PrintEvents;
using Hymson.MES.CoreServices.Events.Quality;
using Hymson.MES.CoreServices.IntegrationEvents.Events.Messages;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.SqlActuator.Services;
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
        private readonly IDeliveryService _deliveryService;
        private readonly IOptions<ManuSfcStepTableOptions> _options;
        private readonly IClearCacheService _clearCacheService;
        private readonly ILogger<SubHostedService> _logger;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventBus"></param>
        /// <param name="clearCacheService"></param>
        public SubHostedService(IEventBus<EventBusInstance1> eventBus,
            IDeliveryService deliveryService,
            IOptions<ManuSfcStepTableOptions> options,
            IClearCacheService clearCacheService,ILogger<SubHostedService> logger)
        {
            _eventBus = eventBus;
            _deliveryService = deliveryService;
            _options = options;
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
                _eventBus.Subscribe<MessageTriggerUpgradeIntegrationEvent, MessageTriggerUpgradeIntegrationEventHandler>();
                _eventBus.Subscribe<MessageReceiveUpgradeIntegrationEvent, MessageReceiveUpgradeIntegrationEventHandler>();
                _eventBus.Subscribe<MessageHandleUpgradeIntegrationEvent, MessageHandleUpgradeIntegrationEventHandler>();
                _eventBus.Subscribe<PrintIntegrationEvent, ExecPrintIntegrationEventHandler>();
                _eventBus.Subscribe<FQCOrderAutoCreateIntegrationEvent, FQCOrderAutoCreateIntegrationEventHandler>();
                SubscribeManufactureServices();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"启动出错:");
            }
        }

        /// <summary>
        /// 生产订阅服务
        /// </summary>
        public void SubscribeManufactureServices()
        {
            _eventBus.Subscribe<ManuSfcStepEvent, ManuSfcStepHander>();
            _eventBus.Subscribe<ManuSfcStepsEvent, ManuSfcStepsHander>();
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
