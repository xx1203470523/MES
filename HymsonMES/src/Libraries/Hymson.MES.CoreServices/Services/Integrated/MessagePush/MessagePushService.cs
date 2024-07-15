using Hymson.EventBus.Abstractions;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Integrated;
using Hymson.MES.CoreServices.Events.IntegrationEvents.Messages;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MessagePush.Enum;
using Hymson.MessagePush.Services;
using Hymson.Utils;
using Microsoft.Extensions.Logging;

namespace Hymson.MES.CoreServices.Services.Integrated
{
    /// <summary>
    /// 类（消息推送服务）
    /// </summary>
    public class MessagePushService : IMessagePushService
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly ILogger<OutStationJobService> _logger;

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
        /// 仓储接口（事件维护）
        /// </summary>
        private readonly IInteEventRepository _inteEventRepository;

        /// <summary>
        /// 仓储接口（事件类型维护）
        /// </summary>
        private readonly IInteEventTypeRepository _inteEventTypeRepository;

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
        /// 仓储接口（事件类型关联群组）
        /// </summary>
        private readonly IInteEventTypeMessageGroupRelationRepository _inteEventTypeMessageGroupRelationRepository;

        /// <summary>
        /// 仓储接口（消息管理）
        /// </summary>
        private readonly IInteMessageManageRepository _inteMessageManageRepository;

        /// <summary>
        /// 仓储接口（工作中心）
        /// </summary>
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;

        /// <summary>
        /// 仓储接口（资源维护）
        /// </summary>
        private readonly IProcResourceRepository _procResourceRepository;

        /// <summary>
        /// 仓储接口（设备注册）
        /// </summary>
        private readonly IEquEquipmentRepository _equEquipmentRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="messageService"></param>
        /// <param name="eventBus"></param>
        /// <param name="messageTemplateRepository"></param>
        /// <param name="inteMessageGroupPushMethodRepository"></param>
        /// <param name="inteEventTypeMessageGroupRelationRepository"></param>
        /// <param name="inteEventRepository"></param>
        /// <param name="inteEventTypeRepository"></param>
        /// <param name="inteEventTypeUpgradeRepository"></param>
        /// <param name="inteEventTypeUpgradeMessageGroupRelationRepository"></param>
        /// <param name="inteEventTypePushRuleRepository"></param>
        /// <param name="inteMessageManageRepository"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="equEquipmentRepository"></param>
        /// <param name="inteWorkCenterRepository"></param>
        public MessagePushService(ILogger<OutStationJobService> logger,
            IMessageService messageService,
            IEventBus<EventBusInstance1> eventBus,
            IMessageTemplateRepository messageTemplateRepository,
            IInteMessageGroupPushMethodRepository inteMessageGroupPushMethodRepository,
            IInteEventTypeMessageGroupRelationRepository inteEventTypeMessageGroupRelationRepository,
            IInteEventRepository inteEventRepository,
            IInteEventTypeRepository inteEventTypeRepository,
            IInteEventTypeUpgradeRepository inteEventTypeUpgradeRepository,
            IInteEventTypeUpgradeMessageGroupRelationRepository inteEventTypeUpgradeMessageGroupRelationRepository,
            IInteEventTypePushRuleRepository inteEventTypePushRuleRepository,
            IInteMessageManageRepository inteMessageManageRepository,
            IProcResourceRepository procResourceRepository,
            IEquEquipmentRepository equEquipmentRepository,
            IInteWorkCenterRepository inteWorkCenterRepository)
        {
            _logger = logger;
            _messageService = messageService;
            _eventBus = eventBus;
            _messageTemplateRepository = messageTemplateRepository;
            _inteMessageGroupPushMethodRepository = inteMessageGroupPushMethodRepository;
            _inteEventRepository = inteEventRepository;
            _inteEventTypeRepository = inteEventTypeRepository;
            _inteEventTypeUpgradeRepository = inteEventTypeUpgradeRepository;
            _inteEventTypeUpgradeMessageGroupRelationRepository = inteEventTypeUpgradeMessageGroupRelationRepository;
            _inteEventTypePushRuleRepository = inteEventTypePushRuleRepository;
            _inteEventTypeMessageGroupRelationRepository = inteEventTypeMessageGroupRelationRepository;
            _inteMessageManageRepository = inteMessageManageRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _procResourceRepository = procResourceRepository;
            _equEquipmentRepository = equEquipmentRepository;
        }

        #region 推送消息
        /// <summary>
        /// 推送消息并添加事件
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task Push(InteMessageManageEntity entity)
        {
            if (entity == null) return;

            // 当前场景
            PushSceneEnum? pushScene = entity.Status switch
            {
                MessageStatusEnum.Trigger => PushSceneEnum.Trigger,
                MessageStatusEnum.Receive => PushSceneEnum.Receive,
                MessageStatusEnum.Handle => PushSceneEnum.Handle,
                MessageStatusEnum.Close => PushSceneEnum.Close,
                _ => null
            };
            if (pushScene == null) return;

            // 查看推送开关
            var eventTypePushRules = await _inteEventTypePushRuleRepository.GetEntitiesAsync(new EntityByParentIdQuery
            {
                SiteId = entity.SiteId,
                ParentId = entity.EventTypeId
            });

            if (eventTypePushRules == null) return;

            // 如果开启的开关不含有该场景，则不发送即时通知
            if (eventTypePushRules.Any(a => a.IsEnabled == DisableOrEnableEnum.Enable && a.PushScene == pushScene.Value))
            {
                // 发送即时消息
                await SendMessageAsync(entity, pushScene.Value);
            }

            // 升级场景（因为只有接收升级和处理升级才推送）
            pushScene = entity.Status switch
            {
                MessageStatusEnum.Trigger => PushSceneEnum.ReceiveUpgrade,
                MessageStatusEnum.Receive => PushSceneEnum.HandleUpgrade,
                _ => null
            };
            if (pushScene == null) return;

            // 如果开启的开关不含有该场景，则不发送即时通知（升级场景）
            if (!eventTypePushRules.Any(a => a.IsEnabled == DisableOrEnableEnum.Enable && a.PushScene == pushScene.Value)) return;
            dynamic? @event = pushScene.Value switch
            {
                PushSceneEnum.ReceiveUpgrade => new MessageReceiveUpgradeIntegrationEvent { MessageId = entity.Id },
                PushSceneEnum.HandleUpgrade => new MessageHandleUpgradeIntegrationEvent { MessageId = entity.Id },
                _ => null
            };
            if (@event == null) return;

            // 读取接收升级等级设置（已缓存）
            var eventTypeUpgrades = await _inteEventTypeUpgradeRepository.GetEntitiesAsync(new InteEventTypeUpgradeQuery
            {
                SiteId = entity.SiteId,
                EventTypeId = entity.EventTypeId,
                PushScene = pushScene.Value
            });
            if (eventTypeUpgrades == null || !eventTypeUpgrades.Any()) return;

            // 当前匹配的等级（这个一定有数据，因为前面已经做了Any()验证
            InteEventTypeUpgradeEntity? firstLevelEntity = eventTypeUpgrades.OrderBy(o => o.Level).FirstOrDefault();
            if (firstLevelEntity != null)
            {
                @event.Level = firstLevelEntity.Level;
                _eventBus.PublishDelay(@event, firstLevelEntity.Duration * 60);
            }
        }

        /// <summary>
        /// 任务回调（触发）
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public async Task TriggerCallBackAsync(MessageTriggerUpgradeIntegrationEvent @event)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// 任务回调（接收）
        /// </summary>
        /// <returns></returns>
        public async Task ReceiveCallBackAsync(MessageReceiveUpgradeIntegrationEvent @event)
        {
            // 查询一次任务状态
            var messageEntity = await _inteMessageManageRepository.GetByIdAsync(@event.MessageId);
            if (messageEntity == null) return;

            // 状态已经变更，不再继续
            if (messageEntity.Status != MessageStatusEnum.Trigger) return;

            // 设置发送升级事件
            await SetNextUpgradeLevelAsync(@event, PushSceneEnum.ReceiveUpgrade, messageEntity);
        }

        /// <summary>
        /// 任务回调（处理）
        /// </summary>
        /// <returns></returns>
        public async Task HandleCallBackAsync(MessageHandleUpgradeIntegrationEvent @event)
        {
            // 查询一次任务状态
            var messageEntity = await _inteMessageManageRepository.GetByIdAsync(@event.MessageId);
            if (messageEntity == null) return;

            // 状态已经变更，不再继续
            if (messageEntity.Status != MessageStatusEnum.Receive) return;

            // 设置发送升级事件
            await SetNextUpgradeLevelAsync(@event, PushSceneEnum.HandleUpgrade, messageEntity);
        }
        #endregion

        #region 内部方法
        /// <summary>
        /// 发送即时消息
        /// </summary>
        /// <param name="messageEntity"></param>
        /// <param name="pushScene"></param>
        /// <returns></returns>
        private async Task SendMessageAsync(InteMessageManageEntity messageEntity, PushSceneEnum pushScene)
        {
            // 读取事件绑定的消息组（已缓存）
            var eventTypeMessageGroupRelations = await _inteEventTypeMessageGroupRelationRepository.GetEntitiesAsync(new EntityByParentIdQuery
            {
                SiteId = messageEntity.SiteId,
                ParentId = messageEntity.EventTypeId
            });
            if (eventTypeMessageGroupRelations == null || !eventTypeMessageGroupRelations.Any()) return;

            // 消息组关联推送方式（已缓存）
            var messageGroupPushMethodEntities = await _inteMessageGroupPushMethodRepository.GetEntitiesAsync(new EntityByParentIdQuery { SiteId = messageEntity.SiteId });
            var messageGroupPushMethodDic = messageGroupPushMethodEntities.ToLookup(w => w.MessageGroupId).ToDictionary(d => d.Key, d => d);

            // 读取模板列表
            var messageTemplateEntities = await _messageTemplateRepository.GetEntitiesAsync(new MessageTemplateQuery
            {
                SiteId = messageEntity.SiteId,
                BusinessType = BusinessTypeEnum.Abnormity
            });
            if (messageTemplateEntities == null || !messageTemplateEntities.Any()) return;

            // 遍历设置的所有推送方式
            foreach (var item in eventTypeMessageGroupRelations)
            {
                var messageTypeArray = item.PushTypes.ToDeserialize<IEnumerable<MessageTypeEnum>>();
                if (messageTypeArray == null) continue;

                // 消息组推送方式
                if (!messageGroupPushMethodDic.TryGetValue(item.MessageGroupId, out var messageGroupPushMethods)) continue;

                foreach (var messageType in messageTypeArray)
                {
                    // 推送方式配置
                    var config = messageGroupPushMethods.FirstOrDefault(f => f.Type == messageType);
                    if (config == null) continue;

                    // 读取对应场景的模板
                    var templateEntity = messageTemplateEntities.FirstOrDefault(f => f.MessageType == messageType && f.PushScene == pushScene);
                    if (templateEntity == null) continue;

                    // 推送即时消息
                    var messagePushBo = await ConvertEntityToMessagePushBoAsync(messageEntity, pushScene);
                    _logger.LogInformation($"发送即时消息：{messageType.GetDescription()}，地址：{config.Address}，内容：{templateEntity.Content}，消息内容：{messagePushBo}");

                    await _messageService.SendMessageAsync(messageType, config.Address, templateEntity.Content, messagePushBo, messageEntity.UpdatedBy ?? messageEntity.CreatedBy);
                }
            }
        }

        /// <summary>
        /// 获取下一等级设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="event"></param>
        /// <param name="pushScene"></param>
        /// <param name="messageEntity"></param>
        /// <returns></returns>
        private async Task SetNextUpgradeLevelAsync<T>(T @event, PushSceneEnum pushScene, InteMessageManageEntity messageEntity) where T : IntegrationEvent
        {
            // 读取接收升级等级设置（已缓存）
            var eventTypeUpgrades = await _inteEventTypeUpgradeRepository.GetEntitiesAsync(new InteEventTypeUpgradeQuery
            {
                SiteId = messageEntity.SiteId,
                EventTypeId = messageEntity.EventTypeId,
                PushScene = pushScene
            });
            if (eventTypeUpgrades == null || !eventTypeUpgrades.Any()) return;

            dynamic dyEvent = @event;

            // 下一升级等级
            var currentEventTypeUpgrade = eventTypeUpgrades.FirstOrDefault(o => o.Level == dyEvent.Level);
            InteEventTypeUpgradeEntity? nextEventTypeUpgrade = eventTypeUpgrades.Where(w => w.Level > dyEvent.Level).OrderBy(o => o.Level).FirstOrDefault();

            // 发送即时消息（升级消息）
            if (currentEventTypeUpgrade != null) await SendUpgradeMessageAsync(messageEntity, pushScene, currentEventTypeUpgrade);

            // 添加升级检查任务
            if (nextEventTypeUpgrade == null) return;
            dyEvent.Level = nextEventTypeUpgrade.Level;

            // 即将检查的等级
            var delayMinute = 1;
            var duration = (HymsonClock.Now() - messageEntity.CreatedOn).TotalMinutes;
            if (duration < nextEventTypeUpgrade.Duration)
            {
                delayMinute = Math.Ceiling(nextEventTypeUpgrade.Duration - duration).ParseToInt(1);
            }

            _eventBus.PublishDelay(dyEvent, delayMinute * 60);
        }

        /// <summary>
        /// 发送即时消息（升级消息）
        /// </summary>
        /// <param name="messageEntity"></param>
        /// <param name="pushScene"></param>
        /// <param name="eventTypeUpgrade"></param>
        /// <returns></returns>
        private async Task SendUpgradeMessageAsync(InteMessageManageEntity messageEntity, PushSceneEnum pushScene, InteEventTypeUpgradeEntity eventTypeUpgrade)
        {
            // 升级消息组关联信息
            var messageGroupRelationEntities = await _inteEventTypeUpgradeMessageGroupRelationRepository.GetEntitiesAsync(new InteEventTypeUpgradeMessageGroupRelationQuery
            {
                SiteId = messageEntity.SiteId,
                EventTypeId = messageEntity.EventTypeId,
                PushScene = pushScene
            });
            // 过滤出当前升级等级的消息组关联信息
            messageGroupRelationEntities = messageGroupRelationEntities.Where(w => w.EventTypeUpgradeId == eventTypeUpgrade.Id);

            // 消息组关联推送方式（已缓存）
            var messageGroupPushMethodEntities = await _inteMessageGroupPushMethodRepository.GetEntitiesAsync(new EntityByParentIdQuery { SiteId = messageEntity.SiteId });
            var messageGroupPushMethodDic = messageGroupPushMethodEntities.ToLookup(w => w.MessageGroupId).ToDictionary(d => d.Key, d => d);

            // 读取模板列表
            var messageTemplateEntities = await _messageTemplateRepository.GetEntitiesAsync(new MessageTemplateQuery
            {
                SiteId = messageEntity.SiteId,
                BusinessType = BusinessTypeEnum.Abnormity
            });
            if (messageTemplateEntities == null || !messageTemplateEntities.Any()) return;

            // 遍历设置的所有推送方式
            foreach (var item in messageGroupRelationEntities)
            {
                // 获取升级等级对应的消息组关联推送方式（已缓存）
                var messageTypeArray = item.PushTypes.ToDeserialize<IEnumerable<MessageTypeEnum>>();
                if (messageTypeArray == null) return;

                // 消息组推送方式
                if (!messageGroupPushMethodDic.TryGetValue(item.MessageGroupId, out var messageGroupPushMethods)) continue;

                foreach (var messageType in messageTypeArray)
                {
                    // 推送方式配置
                    var config = messageGroupPushMethods.FirstOrDefault(f => f.Type == messageType);
                    if (config == null) continue;

                    // 读取对应场景的模板
                    var templateEntity = messageTemplateEntities.FirstOrDefault(f => f.MessageType == messageType && f.PushScene == pushScene);
                    if (templateEntity == null) continue;

                    // 推送即时消息
                    var messagePushBo = await ConvertEntityToMessagePushBoAsync(messageEntity, pushScene);
                    _logger.LogInformation($"发送升级消息：{messageType.GetDescription()}，地址：{config.Address}，内容：{templateEntity.Content}，消息内容：{messagePushBo}");

                    await _messageService.SendMessageAsync(messageType, config.Address, templateEntity.Content, messagePushBo, messageEntity.UpdatedBy ?? messageEntity.CreatedBy);
                }
            }
        }


        /// <summary>
        /// 转换实体为BO对象
        /// </summary>
        /// <param name="messageEntity"></param>
        /// <param name="pushScene"></param>
        /// <returns></returns>
        private async Task<MessagePushBo> ConvertEntityToMessagePushBoAsync(InteMessageManageEntity messageEntity, PushSceneEnum pushScene)
        {
            var eventTypeName = "";
            var eventTypeEntity = await _inteEventTypeRepository.GetByIdAsync(messageEntity.EventTypeId);
            if (eventTypeEntity != null) eventTypeName = eventTypeEntity.Name;

            var workShopName = "";
            var workShopEntity = await _inteWorkCenterRepository.GetByIdAsync(messageEntity.WorkShopId);
            if (workShopEntity != null) workShopName = workShopEntity.Name;

            var workLineName = "";
            var workLineEntity = await _inteWorkCenterRepository.GetByIdAsync(messageEntity.LineId);
            if (workLineEntity != null) workLineName = workLineEntity.Name;

            var messagePushBo = new MessagePushBo
            {
                PushScene = (int)pushScene, // 模板里面不支持枚举
                Code = messageEntity.Code,
                Status = messageEntity.Status.GetDescription(),
                Level = messageEntity.UrgencyLevel.GetDescription(),
                EventTypeName = eventTypeName,
                WorkShopName = workShopName,
                WorkLineName = workLineName,
                TriggerUser = messageEntity.CreatedBy,
                TriggerTime = $"{messageEntity.CreatedOn:yyyy-MM-dd HH:mm:ss}",
            };

            var eventEntity = await _inteEventRepository.GetByIdAsync(messageEntity.EventId);
            if (eventEntity != null) messagePushBo.EventName = eventEntity.Name;

            if (messageEntity.ResourceId.HasValue)
            {
                var resourceEntity = await _procResourceRepository.GetByIdAsync(messageEntity.ResourceId.Value);
                if (resourceEntity != null) messagePushBo.ResourceName = resourceEntity.ResName;
            }

            if (messageEntity.EquipmentId.HasValue)
            {
                var equipmentEntity = await _equEquipmentRepository.GetByIdAsync(messageEntity.EquipmentId.Value);
                if (equipmentEntity != null) messagePushBo.EquipmentName = equipmentEntity.EquipmentName;
            }

            if (messageEntity.EventName != null) messagePushBo.EventName = messageEntity.EventName;
            if (messageEntity.UpdatedBy != null) messagePushBo.ReceiveUser = messageEntity.UpdatedBy;
            if (messageEntity.UpdatedOn != null) messagePushBo.ReceiveTime = $"{messageEntity.UpdatedOn.Value:yyyy-MM-dd HH:mm:ss}";
            if (messageEntity.EvaluateBy != null) messagePushBo.EvaluateBy = messageEntity.EvaluateBy;
            if (messageEntity.EvaluateOn != null) messagePushBo.EvaluateOn = messageEntity.EvaluateOn;

            return messagePushBo;
        }
        #endregion

    }
}
