﻿using Hymson.EventBus.Abstractions;

namespace Hymson.MES.CoreServices.IntegrationEvents.Events.Messages
{
    /// <summary>
    /// 消息触发成功事件
    /// </summary>
    public record MessageTriggerUpgradeIntegrationEvent : IntegrationEvent
    {
        /// <summary>
        /// 消息Id
        /// </summary>
        public long MessageId { get; set; }

    }
}
