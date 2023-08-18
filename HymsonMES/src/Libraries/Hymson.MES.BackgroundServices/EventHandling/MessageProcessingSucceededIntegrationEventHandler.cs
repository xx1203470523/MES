using Hymson.EventBus.Abstractions;
using Hymson.MES.CoreServices.IntegrationEvents.Events.Messages;
using Hymson.MES.CoreServices.Services.Integrated;

namespace Hymson.MES.BackgroundServices.EventHandling
{
    /// <summary>
    /// 事件总线回调（消息处理成功）
    /// </summary>
    public class MessageProcessingSucceededIntegrationEventHandler : IIntegrationEventHandler<MessageProcessingSucceededIntegrationEvent>
    {
        /// <summary>
        /// 消息服务
        /// </summary>
        private readonly IMessagePushService _messagePushService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="messagePushService"></param>
        public MessageProcessingSucceededIntegrationEventHandler(IMessagePushService messagePushService)
        {
            _messagePushService = messagePushService;
        }

        /// <summary>
        /// 处理方法
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public async Task Handle(MessageProcessingSucceededIntegrationEvent @event)
        {
            await _messagePushService.HandleCallBackAsync(@event);
        }
    }
}
