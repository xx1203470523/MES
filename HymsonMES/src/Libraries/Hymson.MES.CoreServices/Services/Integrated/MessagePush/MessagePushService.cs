using Hymson.EventBus.Abstractions;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Integrated;
using Hymson.MES.CoreServices.IntegrationEvents.Events.Messages;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Hymson.MessagePush.Enum;
using Hymson.MessagePush.Services;
using Hymson.Utils;

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
        /// 仓储接口（消息模板）
        /// </summary>
        private readonly IMessageTemplateRepository _messageTemplateRepository;

        /// <summary>
        /// 仓储接口（消息组推送方式）
        /// </summary>
        private readonly IInteMessageGroupPushMethodRepository _inteMessageGroupPushMethodRepository;

        /// <summary>
        /// 仓储接口（事件类型关联群组）
        /// </summary>
        private readonly IInteEventTypeMessageGroupRelationRepository _inteEventTypeMessageGroupRelationRepository;

        /// <summary>
        /// 仓储接口（事件升级）
        /// </summary>
        private readonly IInteEventTypeUpgradeRepository _inteEventTypeUpgradeRepository;

        /// <summary>
        /// 仓储接口（事件类型推送规则）
        /// </summary>
        private readonly IInteEventTypePushRuleRepository _inteEventTypePushRuleRepository;

        /// <summary>
        /// 仓储接口（消息管理）
        /// </summary>
        private readonly IInteMessageManageRepository _inteMessageManageRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="messageService"></param>
        /// <param name="eventBus"></param>
        /// <param name="messageTemplateRepository"></param>
        /// <param name="inteMessageGroupPushMethodRepository"></param>
        /// <param name="inteEventTypeMessageGroupRelationRepository"></param>
        /// <param name="inteEventTypeUpgradeRepository"></param>
        /// <param name="inteEventTypePushRuleRepository"></param>
        /// <param name="inteMessageManageRepository"></param>
        public MessagePushService(IMessageService messageService, IEventBus<EventBusInstance1> eventBus,
            IMessageTemplateRepository messageTemplateRepository,
            IInteMessageGroupPushMethodRepository inteMessageGroupPushMethodRepository,
            IInteEventTypeMessageGroupRelationRepository inteEventTypeMessageGroupRelationRepository,
            IInteEventTypeUpgradeRepository inteEventTypeUpgradeRepository,
            IInteEventTypePushRuleRepository inteEventTypePushRuleRepository,
            IInteMessageManageRepository inteMessageManageRepository)
        {
            _messageService = messageService;
            _eventBus = eventBus;
            _messageTemplateRepository = messageTemplateRepository;
            _inteMessageGroupPushMethodRepository = inteMessageGroupPushMethodRepository;
            _inteEventTypeMessageGroupRelationRepository = inteEventTypeMessageGroupRelationRepository;
            _inteEventTypeUpgradeRepository = inteEventTypeUpgradeRepository;
            _inteEventTypePushRuleRepository = inteEventTypePushRuleRepository;
            _inteMessageManageRepository = inteMessageManageRepository;
        }

        #region 推送消息
        /// <summary>
        /// 推送消息并添加事件
        /// </summary>
        /// <param name="messageEntity"></param>
        /// <returns></returns>
        public async Task Push(InteMessageManageEntity messageEntity)
        {
            if (messageEntity == null) return;

            // 推送场景
            switch (messageEntity.Status)
            {
                case MessageStatusEnum.Trigger:
                    // 发送即时消息
                    await SetMessageAsync(messageEntity, PushSceneEnum.Trigger);

                    // 设置发送升级事件
                    await SetNextUpgradeLevelAsync(new MessageReceiveUpgradeEvent { MessageId = messageEntity.Id }, messageEntity, PushSceneEnum.ReceiveUpgrade);
                    break;
                case MessageStatusEnum.Receive:
                    // 发送即时消息
                    await SetMessageAsync(messageEntity, PushSceneEnum.Receive);

                    // 设置发送升级事件
                    await SetNextUpgradeLevelAsync(new MessageHandleUpgradeEvent { MessageId = messageEntity.Id }, messageEntity, PushSceneEnum.HandleUpgrade);
                    break;
                case MessageStatusEnum.Handle:
                case MessageStatusEnum.Close:
                default:
                    break;
            }


        }

        /// <summary>
        /// 任务回调（触发）
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public async Task TriggerCallBackAsync(MessageTriggerUpgradeEvent @event)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// 任务回调（接收）
        /// </summary>
        /// <returns></returns>
        public async Task ReceiveCallBackAsync(MessageReceiveUpgradeEvent @event)
        {
            // 查询一次任务状态
            var messageEntity = await _inteMessageManageRepository.GetByIdAsync(@event.EventId);
            if (messageEntity == null) return;

            // 状态已经变更，不再继续
            if (messageEntity.Status != MessageStatusEnum.Trigger) return;

            // 发送即时消息
            await SetMessageAsync(messageEntity, PushSceneEnum.ReceiveUpgrade);

            // 设置发送升级事件
            await SetNextUpgradeLevelAsync(new MessageReceiveUpgradeEvent { MessageId = @event.MessageId }, messageEntity, PushSceneEnum.ReceiveUpgrade);
        }

        /// <summary>
        /// 任务回调（处理）
        /// </summary>
        /// <returns></returns>
        public async Task HandleCallBackAsync(MessageHandleUpgradeEvent @event)
        {
            // 查询一次任务状态
            var messageEntity = await _inteMessageManageRepository.GetByIdAsync(@event.EventId);
            if (messageEntity == null) return;

            // 状态已经变更，不再继续
            if (messageEntity.Status != MessageStatusEnum.Receive) return;

            // 发送即时消息
            await SetMessageAsync(messageEntity, PushSceneEnum.HandleUpgrade);

            // 设置发送升级事件
            await SetNextUpgradeLevelAsync(new MessageHandleUpgradeEvent { MessageId = @event.MessageId }, messageEntity, PushSceneEnum.HandleUpgrade);
        }
        #endregion

        #region 内部方法
        /// <summary>
        /// 发送即时消息
        /// </summary>
        /// <param name="messageEntity"></param>
        /// <param name="pushScene"></param>
        /// <returns></returns>
        private async Task SetMessageAsync(InteMessageManageEntity messageEntity, PushSceneEnum pushScene)
        {
            // 只有这两种场景才有发送即时通知需求
            switch (pushScene)
            {
                case PushSceneEnum.Trigger:
                case PushSceneEnum.Receive:
                case PushSceneEnum.ReceiveUpgrade:
                case PushSceneEnum.Handle:
                case PushSceneEnum.HandleUpgrade:
                    break;
                case PushSceneEnum.Close:
                default: return;
            }

            // 查看推送开关
            var eventTypePushRules = await _inteEventTypePushRuleRepository.GetEntitiesAsync(new EntityByParentIdQuery
            {
                SiteId = messageEntity.SiteId,
                ParentId = messageEntity.EventTypeId
            });

            // 如果开启的开关不含有该场景，则不发送即时通知
            if (eventTypePushRules == null || eventTypePushRules.Any(a => a.IsEnabled == DisableOrEnableEnum.Enable && a.PushScene == pushScene) == false) return;

            // 读取事件绑定的消息组（已缓存）
            var eventTypeMessageGroupRelations = await _inteEventTypeMessageGroupRelationRepository.GetEntitiesAsync(new EntityByParentIdQuery
            {
                SiteId = messageEntity.SiteId,
                ParentId = messageEntity.EventTypeId
            });
            if (eventTypeMessageGroupRelations == null || eventTypeMessageGroupRelations.Any() == false) return;

            // 消息组关联推送方式（已缓存）
            var messageGroupPushMethodEntities = await _inteMessageGroupPushMethodRepository.GetEntitiesAsync(new EntityByParentIdQuery { SiteId = messageEntity.SiteId });
            var messageGroupPushMethodDic = messageGroupPushMethodEntities.ToLookup(w => w.MessageGroupId).ToDictionary(d => d.Key, d => d);

            // 读取模板列表
            var messageTemplateEntities = await _messageTemplateRepository.GetEntitiesAsync(new MessageTemplateQuery
            {
                SiteId = messageEntity.SiteId,
                BusinessType = BusinessTypeEnum.Abnormity
            });
            if (messageTemplateEntities == null || messageTemplateEntities.Any() == false) return;

            // 遍历设置的所有推送方式
            foreach (var item in eventTypeMessageGroupRelations)
            {
                var pushTypeArray = item.PushTypes.ToDeserialize<IEnumerable<MessageTypeEnum>>();
                if (pushTypeArray == null) continue;

                // 消息组推送方式
                if (messageGroupPushMethodDic.TryGetValue(item.MessageGroupId, out var messageGroupPushMethods) == false) continue;

                foreach (var pushType in pushTypeArray)
                {
                    // 推送方式配置
                    var config = messageGroupPushMethods.FirstOrDefault(f => f.Type == pushType);
                    if (config == null) continue;

                    // 读取对应场景的模板
                    var templateEntity = messageTemplateEntities.FirstOrDefault(f => f.MessageType == pushType && f.PushScene == pushScene);
                    if (templateEntity == null) continue;

                    // 推送即时消息
                    await _messageService.SendMessageAsync(pushType, config.Address, templateEntity.Content, new MessagePushBo
                    {
                        Code = messageEntity.Code
                    }, messageEntity.UpdatedBy ?? messageEntity.CreatedBy);
                }
            }
        }

        /// <summary>
        /// 获取下一等级设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="event"></param>
        /// <param name="messageEntity"></param>
        /// <param name="pushScene"></param>
        /// <returns></returns>
        private async Task SetNextUpgradeLevelAsync<T>(T @event, InteMessageManageEntity messageEntity, PushSceneEnum pushScene) where T : IntegrationEvent
        {
            // 即将检查的等级
            InteEventTypeUpgradeEntity? eventTypeUpgrade = null;

            // 读取接收升级等级设置（已缓存）
            var eventTypeUpgrades = await _inteEventTypeUpgradeRepository.GetEntitiesAsync(new InteEventTypeUpgradeQuery
            {
                SiteId = messageEntity.SiteId,
                EventTypeId = messageEntity.EventTypeId,
                PushScene = pushScene
            });
            if (eventTypeUpgrades == null || eventTypeUpgrades.Any() == false) return;

            // 即将检查的等级
            var nowTime = HymsonClock.Now();
            var duration = (nowTime - messageEntity.CreatedOn).TotalMinutes;

            switch (pushScene)
            {
                case PushSceneEnum.Trigger:
                case PushSceneEnum.Receive:
                    // 先从级别最低的开始
                    eventTypeUpgrade = eventTypeUpgrades.OrderBy(o => o.Level).FirstOrDefault();
                    break;
                case PushSceneEnum.ReceiveUpgrade:
                case PushSceneEnum.HandleUpgrade:
                    // 下一升级等级
                    eventTypeUpgrade = eventTypeUpgrades.Where(w => w.Duration >= duration).OrderBy(o => o.Level).FirstOrDefault();
                    break;
                case PushSceneEnum.Handle:
                case PushSceneEnum.Close:
                default:
                    break;
            }

            if (eventTypeUpgrade == null) return;

            // 只有这两种场景才有升级需求
            if (pushScene != PushSceneEnum.ReceiveUpgrade && pushScene != PushSceneEnum.HandleUpgrade) return;

            // 添加升级检查任务
            _eventBus.PublishDelay(@event, eventTypeUpgrade.Duration * 60);
        }
        #endregion

    }
}
