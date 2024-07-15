using Hymson.EventBus.Abstractions;
using Hymson.MES.CoreServices.Events.IntegrationEvents.Messages;
using Hymson.MES.CoreServices.Services.Integrated;

namespace Hymson.MES.BackgroundServices.EventHandling
{
    /// <summary>
    /// 事件总线回调（消息处理成功）
    /// </summary>
    public class MessageHandleUpgradeIntegrationEventHandler : IIntegrationEventHandler<MessageHandleUpgradeIntegrationEvent>
    {
        /// <summary>
        /// 消息服务
        /// </summary>
        private readonly IMessagePushService _messagePushService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="messagePushService"></param>
        public MessageHandleUpgradeIntegrationEventHandler(IMessagePushService messagePushService)
        {
            _messagePushService = messagePushService;
        }

        /// <summary>
        /// 处理方法
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public async Task Handle(MessageHandleUpgradeIntegrationEvent @event)
        {
            await _messagePushService.HandleCallBackAsync(@event);
        }
    }
}
