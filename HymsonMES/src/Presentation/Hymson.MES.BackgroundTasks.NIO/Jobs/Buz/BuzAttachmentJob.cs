using Hymson.MES.BackgroundServices.NIO.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hymson.MES.BackgroundTasks.NIO
{
    /// <summary>
    /// 业务数据（附件）
    /// </summary>
    [DisallowConcurrentExecution]
    internal class BuzAttachmentJob : IJob
    {
        private readonly ILogger<BuzAttachmentJob> _logger;
        private readonly IBuzDataPushService _buzDataPushService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="buzDataPushService"></param>
        public BuzAttachmentJob(ILogger<BuzAttachmentJob> logger, IBuzDataPushService buzDataPushService)
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
                await _buzDataPushService.AttachmentAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "扫描推送数据 -> 业务数据（附件）:");
            }
        }

    }
}
