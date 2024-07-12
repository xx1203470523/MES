using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.CoreServices.Events.IntegrationEvents.Messages;

namespace Hymson.MES.CoreServices.Services.Integrated
{
    /// <summary>
    /// 接口（消息推送服务）
    /// </summary>
    public interface IMessagePushService
    {
        /// <summary>
        /// 推送消息并添加事件总线
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Push(InteMessageManageEntity entity);

        /// <summary>
        /// 任务回调（触发）
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        Task TriggerCallBackAsync(MessageTriggerUpgradeIntegrationEvent @event);

        /// <summary>
        /// 任务回调（接收）
        /// </summary>
        /// <returns></returns>
        Task ReceiveCallBackAsync(MessageReceiveUpgradeIntegrationEvent @event);

        /// <summary>
        /// 任务回调（处理）
        /// </summary>
        /// <returns></returns>
        Task HandleCallBackAsync(MessageHandleUpgradeIntegrationEvent @event);

    }
}
