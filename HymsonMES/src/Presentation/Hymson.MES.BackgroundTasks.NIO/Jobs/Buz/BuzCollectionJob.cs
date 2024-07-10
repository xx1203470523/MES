using Hymson.MES.BackgroundServices.NIO.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hymson.MES.BackgroundTasks.NIO
{
    /// <summary>
    /// 业务数据（控制项）
    /// </summary>
    [DisallowConcurrentExecution]
    internal class BuzCollectionJob : IJob
    {
        private readonly ILogger<BuzCollectionJob> _logger;
        private readonly IBuzDataPushService _buzDataPushService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="buzDataPushService"></param>
        public BuzCollectionJob(ILogger<BuzCollectionJob> logger, IBuzDataPushService buzDataPushService)
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
                await _buzDataPushService.CollectionAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "推送 -> 业务数据（控制项）:");
            }
        }

    }
}
