using Hymson.Logging.Services;
using Hymson.Print.Abstractions;
using Quartz;

namespace Hymson.MES.BackgroundTasks.Jobs
{
    [DisallowConcurrentExecution]
    internal class PrintExecuteJob : IJob
    {
        private readonly IPrintBackgroundService _printBackgroundService;
        private readonly IAlarmLogService _alarmLogService;

        public PrintExecuteJob(IPrintBackgroundService printBackgroundService, IAlarmLogService alarmLogService)
        {
            _printBackgroundService = printBackgroundService;
            _alarmLogService = alarmLogService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _printBackgroundService.BackgroundExecuteAsync(10,true);
            }
            catch (Exception ex)
            {
                _alarmLogService.WriteAlarmLogEntry(new Logging.AlarmLogEntry("后台执行打印出错:" + ex.Message, ex.StackTrace ?? ""));
            }
        }
    }
}
