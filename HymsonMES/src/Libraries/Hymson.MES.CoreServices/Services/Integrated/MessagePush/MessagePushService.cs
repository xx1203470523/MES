using Hymson.EventBus.Abstractions;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.CoreServices.IntegrationEvents.Events.Messages;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MessagePush.Services;

namespace Hymson.MES.CoreServices.Services.Integrated
{
    /// <summary>
    /// 类（消息推送服务）
    /// </summary>
    public class MessagePushService : IMessagePushService
    {
        /// <summary>
        /// 消息服务
        /// </summary>
        private readonly IMessageService _messageService;

        /// <summary>
        /// 事件总线
        /// </summary>
        private readonly IEventBus<EventBusInstance1> _eventBus;

        /// <summary>
        /// 仓储接口（事件类型维护）
        /// </summary>
        private readonly IInteEventTypeRepository _inteEventTypeRepository;

        /// <summary>
        /// 仓储接口（事件类型关联群组）
        /// </summary>
        private readonly IInteEventTypeMessageGroupRelationRepository _inteEventTypeMessageGroupRelationRepository;

        /// <summary>
        /// 仓储接口（事件升级）
        /// </summary>
        private readonly IInteEventTypeUpgradeRepository _inteEventTypeUpgradeRepository;

        /// <summary>
        /// 仓储接口（事件升级消息组关联表）
        /// </summary>
        private readonly IInteEventTypeUpgradeMessageGroupRelationRepository _inteEventTypeUpgradeMessageGroupRelationRepository;

        /// <summary>
        /// 仓储接口（事件类型推送规则）
        /// </summary>
        private readonly IInteEventTypePushRuleRepository _inteEventTypePushRuleRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="messageService"></param>
        /// <param name="eventBus"></param>
        /// <param name="inteEventTypeRepository"></param>
        public MessagePushService(IMessageService messageService, IEventBus<EventBusInstance1> eventBus,
            IInteEventTypeRepository inteEventTypeRepository,
            IInteEventTypeMessageGroupRelationRepository inteEventTypeMessageGroupRelationRepository,
            IInteEventTypeUpgradeRepository inteEventTypeUpgradeRepository,
            IInteEventTypeUpgradeMessageGroupRelationRepository inteEventTypeUpgradeMessageGroupRelationRepository,
            IInteEventTypePushRuleRepository inteEventTypePushRuleRepository)
        {
            _messageService = messageService;
            _eventBus = eventBus;
            _inteEventTypeRepository = inteEventTypeRepository;
            _inteEventTypeMessageGroupRelationRepository = inteEventTypeMessageGroupRelationRepository;
            _inteEventTypeUpgradeRepository = inteEventTypeUpgradeRepository;
            _inteEventTypeUpgradeMessageGroupRelationRepository = inteEventTypeUpgradeMessageGroupRelationRepository;
            _inteEventTypePushRuleRepository = inteEventTypePushRuleRepository;
        }


        #region 推送消息
        /// <summary>
        /// 推送消息并添加事件总线
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task Push(InteMessageManageEntity entity)
        {
            if (entity == null) return;

            // 根据事件类型查询出时间的设置，如首次推送设置，升级设置
            // TODO

            // 读取事件类型
            var eventTypeEntity = await _inteEventTypeRepository.GetByIdAsync(entity.EventTypeId);
            if (eventTypeEntity == null) return;

            // 读取事件绑定的消息组
            var eventTypeMessageGroupRelations = await _inteEventTypeMessageGroupRelationRepository.GetEntitiesAsync(new EntityByParentIdQuery
            {
                SiteId = entity.SiteId,
                ParentId = entity.Id
            });

            /*
            switch (entity.Status)
            {
                case Core.Enums.MessageStatusEnum.Trigger:
                    _eventBus.PublishDelay(new MessageTriggerSucceededIntegrationEvent { }, 30);
                    break;
                case Core.Enums.MessageStatusEnum.Receive:
                    break;
                case Core.Enums.MessageStatusEnum.Handle:
                    break;
                case Core.Enums.MessageStatusEnum.Close:
                    break;
                default:
                    break;
            }
            */

            //await _messageService.SendMessageAsync(MessageTypeEnum.WeChat, "dingding", "Content", entity.CreatedBy);
        }


        /// <summary>
        /// 任务回调（触发）
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public async Task TriggerCallBackAsync(MessageTriggerSucceededIntegrationEvent @event)
        {
            await Task.CompletedTask;
            // TODO 检查状态是否有变更为"接收"，如果没有的话，就发送消息，并添加下一级别Task
        }

        /// <summary>
        /// 任务回调（接收）
        /// </summary>
        /// <returns></returns>
        public async Task ReceiveCallBackAsync(MessageReceiveSucceededIntegrationEvent @event)
        {
            await Task.CompletedTask;
            // TODO 检查状态是否有变更为"处理"，如果没有的话，就发送消息，并添加下一级别Task
        }

        /// <summary>
        /// 任务回调（处理）
        /// </summary>
        /// <returns></returns>
        public async Task HandleCallBackAsync(MessageProcessingSucceededIntegrationEvent @event)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// 任务回调（关闭）
        /// </summary>
        /// <returns></returns>
        public async Task CloseCallBackAsync(MessageCloseSucceededIntegrationEvent @event)
        {
            await Task.CompletedTask;
        }
        #endregion

    }
}
