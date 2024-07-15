using Hymson.MES.BackgroundServices.NIO.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hymson.MES.BackgroundTasks.NIO
{
    /// <summary>
    /// Mock（Hello）
    /// </summary>
    [DisallowConcurrentExecution]
    internal class MockHelloJob : IJob
    {
        private readonly ILogger<MockHelloJob> _logger;
        private readonly IMockDataPushService _mockDataPushService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mockDataPushService"></param>
        public MockHelloJob(ILogger<MockHelloJob> logger, IMockDataPushService mockDataPushService)
        {
            _logger = logger;
            _mockDataPushService = mockDataPushService;
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
                await _mockDataPushService.HelloAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "扫描推送数据 -> Mock（Hello）:");
            }
        }

    }
}
