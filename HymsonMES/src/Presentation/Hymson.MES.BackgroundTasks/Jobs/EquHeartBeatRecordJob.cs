using Hymson.MES.BackgroundServices.Services.EquHeartbeat;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hymson.MES.BackgroundTasks.Jobs
{
    internal class EquHeartBeatRecordJob : IJob
    {
        private readonly ILogger<EquHeartBeatRecordJob> _logger;
        private readonly IEquHeartbeatService _equHeartbeatService;

        public EquHeartBeatRecordJob(ILogger<EquHeartBeatRecordJob> logger, IEquHeartbeatService equHeartbeatService)
        {
            _logger = logger;
            _equHeartbeatService = equHeartbeatService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            int months = 3;
            await _equHeartbeatService.DeleteMonthsBeforeAsync(months);
            _logger.LogInformation("Delete EquHeartBeatRecord {months} Months Before Execute", months);
        }
    }
}
