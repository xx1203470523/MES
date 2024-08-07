using Hymson.MES.BackgroundServices.Stator.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hymson.MES.BackgroundTasks.Stator
{
    /// <summary>
    /// 工序（OP340）
    /// </summary>
    [DisallowConcurrentExecution]
    internal class OP340Job : IJob
    {
        private readonly ILogger<OP340Job> _logger;
        private readonly IOP340Service _opService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="opService"></param>
        public OP340Job(ILogger<OP340Job> logger, IOP340Service opService)
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
                //_ = await _opService.ExecuteAsync(500);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OP340Job :");
            }
        }

    }
}
