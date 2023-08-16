﻿using Hymson.EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.IntegrationEvents.Events.Messages
{
    /// <summary>
    /// 消息处理成功事件
    /// </summary>
    public record MessageProcessingSucceededIntegrationEvent : IntegrationEvent
    {
    }
}