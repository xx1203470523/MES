using Hymson.ClearCache;
using Hymson.EventBus.Abstractions;
using Hymson.Infrastructure.Enums;
using Hymson.MES.BackgroundServices.EventHandling;
using Hymson.MES.BackgroundServices.EventHandling.ManufactureHandling.ManuSfcStepHandling;
using Hymson.MES.CoreServices.Events.ManufactureEvents.ManuSfcStepEvents;
using Hymson.MES.CoreServices.IntegrationEvents.Events.Messages;
using Microsoft.Extensions.Hosting;

namespace Hymson.MES.BackgroundTasks.HostedServices
{
    /// <summary>
    /// 
    /// </summary>
    public class SubHostedService : IHostedService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IEventBus<EventBusInstance1> _eventBus;
        private readonly IClearCacheService _clearCacheService;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventBus"></param>
        /// <param name="clearCacheService"></param>
        public SubHostedService(IEventBus<EventBusInstance1> eventBus,IClearCacheService clearCacheService)
        {
            _eventBus = eventBus;
            _clearCacheService = clearCacheService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _eventBus.Subscribe<MessageTriggerUpgradeIntegrationEvent, MessageTriggerUpgradeIntegrationEventHandler>();
            _eventBus.Subscribe<MessageReceiveUpgradeIntegrationEvent, MessageReceiveUpgradeIntegrationEventHandler>();
            _eventBus.Subscribe<MessageHandleUpgradeIntegrationEvent, MessageHandleUpgradeIntegrationEventHandler>();

            SubscribeManufactureServices();

            try
            {
                await _clearCacheService.ClearCacheAsync(new ServiceTypeEnum[] { ServiceTypeEnum.MES }, cancellationToken);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
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
