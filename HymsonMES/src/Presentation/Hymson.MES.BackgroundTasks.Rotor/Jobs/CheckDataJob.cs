using Hymson.MES.BackgroundServices.Rotor.Services;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundTasks.Rotor.Jobs
{
    [DisallowConcurrentExecution]
    internal class CheckDataJob : IJob
    {
        private readonly ICheckDataService _checkDataService;
        private readonly ILogger<CheckDataJob> _logger;

        public CheckDataJob(ICheckDataService checkDataService, ILogger<CheckDataJob> logger)
        {
            _checkDataService = checkDataService;
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _checkDataService.Check(10);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "后台执行sql语句出错:");
            }
        }
    }
}
