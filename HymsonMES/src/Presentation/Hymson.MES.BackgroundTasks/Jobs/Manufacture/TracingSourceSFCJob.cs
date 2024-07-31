using Hymson.Logging.Services;
using Hymson.MES.BackgroundServices.Tasks.Manufacture.TracingSourceSFC;
using Hymson.SqlActuator.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hymson.MES.BackgroundTasks.Jobs.Manufacture
{
    /// <summary>
    /// 条码追溯任务
    /// </summary>
    [DisallowConcurrentExecution]
    internal class TracingSourceSFCJob : IJob
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<TracingSourceSFCJob> _logger;
        private readonly IAlarmLogService _alarmLogService;
        private readonly ITracingSourceSFCService _tracingSourceSFCService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="alarmLogService"></param>
        /// <param name="tracingSourceSFCService"></param>
        public TracingSourceSFCJob(ILogger<TracingSourceSFCJob> logger,
            IAlarmLogService alarmLogService,
            ITracingSourceSFCService tracingSourceSFCService)
        {
            _logger = logger;
            _alarmLogService = alarmLogService;
            _tracingSourceSFCService = tracingSourceSFCService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.LogDebug("条码追溯开始");
                await _tracingSourceSFCService.ExecuteAsync(500);
            }
            catch (Exception ex)
            {
                _alarmLogService.WriteAlarmLogEntry(new Logging.AlarmLogEntry("条码追溯出错:" + ex.Message, ex.StackTrace ?? ""));
            }
        }

    }
}
