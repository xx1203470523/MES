using Hymson.EventBus.Abstractions;
using Hymson.MES.BackgroundServices.Tasks.Equipment.GenerateMaintenance;
using Hymson.MES.BackgroundServices.Tasks.Equipment.GenerateSpotCheck;
using Hymson.MES.CoreServices.Events.Equipment;
using Hymson.MES.CoreServices.Services.EquMaintenancePlan;
using Hymson.MES.CoreServices.Services.EquSpotcheckPlan;
using NETCore.Encrypt;
using Quartz;

namespace Hymson.MES.BackgroundServices.Quality.EquMaintenanceAutoCreateIntegration
{
    public class EquMaintenanceAutoCreateIntegrationEventHandler : IIntegrationEventHandler<EquMaintenanceAutoCreateIntegrationEvent>
    {
        private readonly IScheduler _scheduler;

        public EquMaintenanceAutoCreateIntegrationEventHandler(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public async Task Handle(EquMaintenanceAutoCreateIntegrationEvent @event)
        {
            //使用计划戳当作key
            var jobName = nameof(GenerateMaintenanceJob);
            var jobKey = @event.MaintenancePlanId.ToString();

            //增加参数
            var jobData = new JobDataMap
            {
                //TODO  使用id 不行的  暂时这样 
                { "param", new GenerateEquSpotcheckTaskDto{ SiteId =@event.SiteId,UserName="Auto",SpotCheckPlanId=@event.MaintenancePlanId,ExecType=@event.ExecType} },

            };
            var job = JobBuilder.Create<GenerateMaintenanceJob>()
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
