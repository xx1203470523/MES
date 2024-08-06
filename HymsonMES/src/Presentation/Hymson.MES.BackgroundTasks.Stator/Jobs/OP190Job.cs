using Hymson.MES.BackgroundServices.Stator.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hymson.MES.BackgroundTasks.Stator
{
    /// <summary>
    /// 工序（OP190）
    /// </summary>
    [DisallowConcurrentExecution]
    internal class OP190Job : IJob
    {
        private readonly ILogger<OP190Job> _logger;
        private readonly IOP190Service _op190Service;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="op190Service"></param>
        public OP190Job(ILogger<OP190Job> logger, IOP190Service op190Service)
        {
            _logger = logger;
            _op190Service = op190Service;
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
                _ = await _op190Service.ExecuteAsync(500);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OP190Job :");
            }
        }

    }
}
