using Hymson.EventBus.Abstractions;
using Hymson.MES.BackgroundServices.EventHandling;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventBus"></param>
        public SubHostedService(IEventBus<EventBusInstance1> eventBus)
        {
            _eventBus = eventBus;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _eventBus.Subscribe<MessageTriggerUpgradeIntegrationEvent, MessageTriggerUpgradeIntegrationEventHandler>();
            _eventBus.Subscribe<MessageReceiveUpgradeIntegrationEvent, MessageReceiveUpgradeIntegrationEventHandler>();
            _eventBus.Subscribe<MessageHandleUpgradeIntegrationEvent, MessageHandleUpgradeIntegrationEventHandler>();

            return Task.CompletedTask;
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
