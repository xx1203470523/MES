using Hymson.MES.BackgroundServices.Stator.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hymson.MES.BackgroundTasks.Stator
{
    /// <summary>
    /// 工序（OP070）
    /// </summary>
    [DisallowConcurrentExecution]
    internal class OP070Job : IJob
    {
        private readonly ILogger<OP070Job> _logger;
        private readonly IOP070Service _op070Service;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="op010Service"></param>
        public OP070Job(ILogger<OP070Job> logger, IOP070Service op070Service)
        {
            _logger = logger;
            _op070Service = op070Service;
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
                _ = await _op070Service.ExecuteAsync(500);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OP070Job :");
            }
        }

    }
}
