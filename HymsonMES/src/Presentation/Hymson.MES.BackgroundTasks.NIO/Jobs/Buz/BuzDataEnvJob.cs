using Hymson.MES.BackgroundServices.NIO.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hymson.MES.BackgroundTasks.NIO
{
    /// <summary>
    /// 业务数据（环境业务）
    /// </summary>
    [DisallowConcurrentExecution]
    internal class BuzDataEnvJob : IJob
    {
        private readonly ILogger<BuzDataEnvJob> _logger;
        private readonly IBuzDataPushService _buzDataPushService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="buzDataPushService"></param>
        public BuzDataEnvJob(ILogger<BuzDataEnvJob> logger, IBuzDataPushService buzDataPushService)
        {
            _logger = logger;
            _buzDataPushService = buzDataPushService;
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
                await _buzDataPushService.DataEnvAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "推送 -> 业务数据（环境业务）:");
            }
        }

    }
}
