using Hymson.EventBus.Abstractions;
using Hymson.MES.CoreServices.Events.Equipment;
using Hymson.MES.CoreServices.Services.EquSpotcheckPlan;
using NETCore.Encrypt;
using Quartz;
using Quartz.Impl.Matchers;

namespace Hymson.MES.BackgroundServices.Quality.EquSpotcheckAutoStopIntegration
{
    public class EquSpotcheckAutoStopIntegrationEventHandler : IIntegrationEventHandler<EquSpotcheckAutoStopIntegrationEvent>
    {
        private readonly IScheduler _scheduler;

        public EquSpotcheckAutoStopIntegrationEventHandler(ISchedulerFactory schedulerFactory)
        {
            _scheduler = schedulerFactory.GetScheduler().Result;
        }

        public async Task Handle(EquSpotcheckAutoStopIntegrationEvent @event)
        {
            var jobKey = @event.SpotCheckPlanId.ToString();

            var triggerKey = new TriggerKey(EncryptProvider.Md5(new TriggerKey($"{jobKey}-Trigger").ToString()));
            var triggerKeys = new List<TriggerKey>();

            // 获取所有触发器组的名称
            var triggerGroupNames = _scheduler.GetTriggerGroupNames().Result;

            // 遍历每个触发器组，获取该组内的所有触发器
            foreach (var groupName in triggerGroupNames)
            {
                var groupMatcher = GroupMatcher<TriggerKey>.GroupEquals(groupName);
                var triggers = _scheduler.GetTriggerKeys(groupMatcher).Result;

                foreach (var keyItem in triggers)
                {
                    var triggerRes = _scheduler.GetTrigger(keyItem).Result;

                    if (triggerRes != null)
                    {
                        triggerKeys.Add(triggerRes.Key);
                    }
                }
            }

            if (triggerKeys.Any(it => it.Name == triggerKey.Name && it.Group == triggerKey.Group))
            {
                await _scheduler.UnscheduleJob(triggerKey);
            }
        }
    }
}
