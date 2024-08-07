using Hymson.MES.BackgroundServices.Stator.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hymson.MES.BackgroundTasks.Stator
{
    /// <summary>
    /// 工序（OP210）
    /// </summary>
    [DisallowConcurrentExecution]
    internal class OP210Job : IJob
    {
        private readonly ILogger<OP210Job> _logger;
        private readonly IOP210Service _opService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="opService"></param>
        public OP210Job(ILogger<OP210Job> logger, IOP210Service opService)
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
                _logger.LogError(ex, "OP210Job :");
            }
        }

    }
}
