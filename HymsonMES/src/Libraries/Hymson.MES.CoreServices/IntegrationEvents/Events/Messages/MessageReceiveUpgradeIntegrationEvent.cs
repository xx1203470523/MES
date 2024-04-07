using Hymson.EventBus.Abstractions;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.CoreServices.IntegrationEvents.Events.Messages
{
    /// <summary>
    /// 消息接收成功事件
    /// </summary>
    public record MessageReceiveUpgradeIntegrationEvent : IntegrationEvent
    {
        /// <summary>
        /// 消息Id
        /// </summary>
        public long MessageId { get; set; }

        /// <summary>
        /// 级别;1、第一等级2、第二等级3、第三等级
        /// </summary>
        public UpgradeLevelEnum Level { get; set; }
    }
}
