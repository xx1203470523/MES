using Hymson.MES.BackgroundServices.Rotor.Services;
using Hymson.SqlActuator.Services;
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
    internal class ManuDataJob : IJob
    {
        private readonly IManuInOutBoundService _manuInOutBoundService;
        private readonly ILogger<ManuDataJob> _logger;

        public ManuDataJob(IManuInOutBoundService manuInOutBoundService, ILogger<ManuDataJob> logger)
        {
            _manuInOutBoundService = manuInOutBoundService;
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _manuInOutBoundService.InOutBoundAsync(500);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "后台执行sql语句出错:");
            }
        }
    }
}
