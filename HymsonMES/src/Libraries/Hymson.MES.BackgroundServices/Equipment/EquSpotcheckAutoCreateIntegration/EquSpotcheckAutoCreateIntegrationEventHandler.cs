using Hymson.EventBus.Abstractions;
using Hymson.Infrastructure;
using Hymson.MES.BackgroundServices.Tasks.Equipment.GenerateMaintenance;
using Hymson.MES.BackgroundServices.Tasks.Equipment.GenerateSpotCheck;
using Hymson.MES.CoreServices.Events.Equipment;
using Hymson.MES.CoreServices.Services.EquMaintenancePlan;
using Hymson.MES.CoreServices.Services.EquSpotcheckPlan;
using NETCore.Encrypt;
using Newtonsoft.Json;
using Quartz;
using System.Security.Policy;

namespace Hymson.MES.BackgroundServices.Quality.EquSpotcheckAutoCreateIntegration
{
    public class EquSpotcheckAutoCreateIntegrationEventHandler : IIntegrationEventHandler<EquSpotcheckAutoCreateIntegrationEvent>
    {
        private readonly IScheduler _scheduler;

        public EquSpotcheckAutoCreateIntegrationEventHandler(ISchedulerFactory schedulerFactory)
        {
            _scheduler = schedulerFactory.GetScheduler().Result;
        }

        public async Task Handle(EquSpotcheckAutoCreateIntegrationEvent @event)
        {
            //使用计划戳当作key
            var jobName = nameof(GenerateSpotCheckJob);
            var jobKey = @event.SpotCheckPlanId.ToString();

            var jobData = new JobDataMap
                {
                    //TODO  使用id 不行的  暂时这样 
                    { "param",JsonConvert.SerializeObject( new GenerateEquSpotcheckTaskDto { SiteId = @event.SiteId, UserName = "Auto", SpotCheckPlanId = @event.SpotCheckPlanId, ExecType = @event.ExecType }) },

                };
            var job = JobBuilder.Create<GenerateSpotCheckJob>()
                .WithIdentity(jobKey)
                .SetJobData(jobData)
                .WithDescription(jobName)
                .Build();

            var triggerKey_Add = EncryptProvider.Md5(new TriggerKey($"{jobKey}-Trigger").ToString());
            var trigger = TriggerBuilder.Create()
                .WithIdentity(triggerKey_Add)
                .ForJob(job)
                .WithCronSchedule(@event.CornExpression)
                .StartAt(@event.FirstExecuteTime)
                .Build();
            await _scheduler.ScheduleJob(job, trigger);

        }
    }
}
