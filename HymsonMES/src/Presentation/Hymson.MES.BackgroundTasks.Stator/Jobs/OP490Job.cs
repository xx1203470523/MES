using Hymson.MES.BackgroundServices.Stator.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hymson.MES.BackgroundTasks.Stator
{
    /// <summary>
    /// 工序（OP490）
    /// </summary>
    [DisallowConcurrentExecution]
    internal class OP490Job : IJob
    {
        private readonly ILogger<OP490Job> _logger;
        private readonly IOP490Service _opService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="opService"></param>
        public OP490Job(ILogger<OP490Job> logger, IOP490Service opService)
        {
            _logger = logger;
            _opService = opService;
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
                _ = await _opService.ExecuteAsync(500);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OP490Job :");
            }
        }

    }
}
