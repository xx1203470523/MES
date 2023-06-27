using Hymson.MES.BackgroundServices.Dtos.EquHeartbeat;
using Hymson.MES.BackgroundServices.Services.EquHeartbeat;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hymson.MES.BackgroundTasks.Jobs
{
    internal class EquHeartbeatJob : IJob
    {
        private readonly ILogger<EquHeartbeatJob> _logger;
        private readonly IEquHeartbeatService _equHeartbeatService;

        public EquHeartbeatJob(ILogger<EquHeartbeatJob> logger, IEquHeartbeatService equHeartbeatService)
        {
            _logger = logger;
            _equHeartbeatService = equHeartbeatService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await _equHeartbeatService.EquipmentHeartbeatUpdateAsync(new EquipmentHeartbeatUpdateDto()
            {
                //30秒没心跳就更新为离线
                IntervalSeconds = 30
            });
            _logger.LogInformation("Equipment Heartbeat Update Execute");
        }
    }
}
