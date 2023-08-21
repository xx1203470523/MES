using Hymson.EventBus.Abstractions;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Integrated;

namespace Hymson.MES.CoreServices.IntegrationEvents.Events.Messages
{
    /// <summary>
    /// 消息触发成功事件
    /// </summary>
    public record MessageTriggerSucceededIntegrationEvent : IntegrationEvent
    {
        /// <summary>
        /// 消息状态;1、触发2、接收3、处理4、关闭
        /// </summary>
        public MessageStatusEnum Status { get; set; }

        /// <summary>
        /// 升级对象
        /// </summary>
        public EventTypeUpgradeBo? UpgradeBo { get; set; }

    }
}
