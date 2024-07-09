using Hymson.Logging.Services;
using Hymson.MES.BackgroundServices.Tasks.Manufacture.TracingSourceSFC;
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
        private readonly IAlarmLogService _alarmLogService;
        private readonly ITracingSourceSFCService _tracingSourceSFCService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="tracingSourceSFCService"></param>
        public TracingSourceSFCJob(IAlarmLogService alarmLogService,
            ITracingSourceSFCService tracingSourceSFCService)
        {
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
                await _tracingSourceSFCService.ExecuteAsync(1000);
            }
            catch (Exception ex)
            {
                _alarmLogService.WriteAlarmLogEntry(new Logging.AlarmLogEntry("条码追溯出错:" + ex.Message, ex.StackTrace ?? ""));
            }
        }

    }
}
