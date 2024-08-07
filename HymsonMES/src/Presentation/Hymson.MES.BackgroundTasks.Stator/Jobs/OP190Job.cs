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
        private readonly IOP190Service _opService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="opService"></param>
        public OP190Job(ILogger<OP190Job> logger, IOP190Service opService)
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
                _logger.LogError(ex, "OP190Job :");
            }
        }

    }
}
