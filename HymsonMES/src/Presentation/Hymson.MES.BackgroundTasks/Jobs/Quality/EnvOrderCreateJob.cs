using Hymson.Logging.Services;
using Hymson.MES.BackgroundServices.Tasks.Quality.EnvOrderCreate;
using Quartz;

namespace Hymson.MES.BackgroundTasks.Jobs.Quality
{
    /// <summary>
    /// 环境检验单自动生成
    /// </summary>
    [DisallowConcurrentExecution]
    internal class EnvOrderCreateJob : IJob
    {

        private readonly IAlarmLogService _alarmLogService;
        private readonly IEnvOrderAutoCreateService _envOrderAutoCreateService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="alarmLogService"></param>
        /// <param name="envOrderAutoCreateService"></param>
        public EnvOrderCreateJob(IAlarmLogService alarmLogService, IEnvOrderAutoCreateService envOrderAutoCreateService)
        {

            _alarmLogService = alarmLogService;
            _envOrderAutoCreateService = envOrderAutoCreateService;
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
                await _envOrderAutoCreateService.ExecuteAsync();
            }
            catch (Exception ex)
            {
                _alarmLogService.WriteAlarmLogEntry(new Logging.AlarmLogEntry("环境检验单自动生成出错:" + ex.Message, ex.StackTrace ?? ""));
            }
        }
    }
}
