using Hymson.EventBus.Abstractions;
using Hymson.MES.BackgroundServices.EventHandling;
using Hymson.MES.CoreServices.Bos.Integrated;
using Hymson.MES.CoreServices.IntegrationEvents.Events.Messages;
using Hymson.Utils;
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
            
            _eventBus.Subscribe<MessageTriggerSucceededIntegrationEvent, MessageTriggerSucceededIntegrationEventHandler>();
            _eventBus.Subscribe<MessageReceiveSucceededIntegrationEvent, MessageReceiveSucceededIntegrationEventHandler>();
            _eventBus.Subscribe<MessageProcessingSucceededIntegrationEvent, MessageProcessingSucceededIntegrationEventHandler>();
            _eventBus.Subscribe<MessageCloseSucceededIntegrationEvent, MessageCloseSucceededIntegrationEventHandler>();

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
