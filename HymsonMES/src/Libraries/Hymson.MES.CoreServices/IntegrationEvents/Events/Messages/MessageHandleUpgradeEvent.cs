using Hymson.EventBus.Abstractions;

namespace Hymson.MES.CoreServices.IntegrationEvents.Events.Messages
{
    /// <summary>
    /// 消息处理成功事件
    /// </summary>
    public record MessageHandleUpgradeEvent : IntegrationEvent
    {
        /// <summary>
        /// 消息Id
        /// </summary>
        public long MessageId { get; set; }
    }
}
