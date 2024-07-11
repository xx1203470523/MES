using Microsoft.Extensions.Logging;
using Quartz;

namespace Hymson.MES.BackgroundTasks.Stator
{
    /// <summary>
    /// 工序（OP080）
    /// </summary>
    [DisallowConcurrentExecution]
    internal class OP080Job : IJob
    {
        private readonly ILogger<OP080Job> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        public OP080Job(ILogger<OP080Job> logger)
        {
            _logger = logger;
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
                await Task.CompletedTask;
                //await _masterDataPushService.ProductAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "TODO :");
            }
        }

    }
}
