using Hymson.MES.CoreServices.Services.EquMaintenancePlan;
using Hymson.MES.CoreServices.Services.EquSpotcheckPlan;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.Tasks.Equipment.GenerateMaintenance
{
    /// <summary>
    /// 生成保养作业
    /// </summary>
    public class GenerateMaintenanceJob : IJob
    {
        private readonly IEquMaintenancePlanCoreService _equMaintenancePlanCoreService;

        public GenerateMaintenanceJob(IEquMaintenancePlanCoreService equMaintenancePlanCoreService)
        {
            _equMaintenancePlanCoreService = equMaintenancePlanCoreService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var jobDataObj = context.JobDetail.JobDataMap.Get("param").ToString();
            if (!string.IsNullOrWhiteSpace(jobDataObj))
            {
                if (JsonConvert.DeserializeObject<GenerateEquMaintenanceTaskDto>(jobDataObj) is GenerateEquMaintenanceTaskDto jobData)
                {
                    await _equMaintenancePlanCoreService.GenerateEquMaintenanceTaskAsync(jobData);
                }
                else
                {

                }
            }

        }
    }
}
