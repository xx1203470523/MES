using Hymson.MES.BackgroundServices.NIO.Services;
using Hymson.Utils;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hymson.MES.BackgroundTasks.NIO
{
    /// <summary>
    /// 推送蔚来作业
    /// </summary>
    [DisallowConcurrentExecution]
    internal class PushNIOJob : IJob
    {
        private readonly ILogger<PushNIOJob> _logger;
        private readonly IPushNIOService _pushNIOService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="pushNIOService"></param>
        public PushNIOJob(ILogger<PushNIOJob> logger, IPushNIOService pushNIOService)
        {
            _logger = logger;
            _pushNIOService = pushNIOService;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.LogInformation($"【PushNIOJob】推送NIO的定时任务 -> 入口");
                await _pushNIOService.ExecutePushAsync(250);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "推送 -> NIO:");
            }
        }

    }
}
