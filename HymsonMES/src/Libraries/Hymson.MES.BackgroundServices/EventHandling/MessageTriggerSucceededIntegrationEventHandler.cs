using Hymson.EventBus.Abstractions;
using Hymson.MES.CoreServices.IntegrationEvents.Events.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.EventHandling
{
    public class MessageTriggerSucceededIntegrationEventHandler : IIntegrationEventHandler<MessageTriggerSucceededIntegrationEvent>
    {
        public Task Handle(MessageTriggerSucceededIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}
