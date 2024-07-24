using Hymson.Logging.Services;
using Hymson.SqlActuator.Services;
using Quartz;

namespace Hymson.MES.BackgroundTasks.Jobs
{
    [DisallowConcurrentExecution]
    internal class SqlExecuteJob : IJob
    {
        private readonly ISqlExecuteTaskService _sqlExecuteTaskService;
        private readonly IAlarmLogService _alarmLogService;

        public SqlExecuteJob(ISqlExecuteTaskService sqlExecuteTaskService, IAlarmLogService alarmLogService)
        {
                _sqlExecuteTaskService = sqlExecuteTaskService;
            _alarmLogService = alarmLogService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _sqlExecuteTaskService.BackgroundExecuteAsync(100);
            }
            catch (Exception ex)
            {
                _alarmLogService.WriteAlarmLogEntry(new Logging.AlarmLogEntry("后台执行sql语句出错:" + ex.Message, ex.StackTrace ?? ""));
            }
        }
    }
}
