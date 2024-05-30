using Hymson.MES.CoreServices.Services.EquSpotcheckPlan;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.Tasks.Equipment.GenerateSpotCheck
{
    /// <summary>
    ///生成点检作业
    /// </summary>
    public class GenerateSpotCheckJob : IJob
    {
        private readonly IEquSpotcheckPlanCoreService _equSpotcheckPlanCoreService;

        public GenerateSpotCheckJob(IEquSpotcheckPlanCoreService equSpotcheckPlanCoreService)
        {
            _equSpotcheckPlanCoreService = equSpotcheckPlanCoreService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var jobDataObj = context.JobDetail.JobDataMap.Get("param");
            if (jobDataObj is GenerateEquSpotcheckTaskDto jobData)
            {
                await _equSpotcheckPlanCoreService.GenerateEquSpotcheckTaskAsync(jobData);
            }
            else
            {
            }
        }
    }
}
