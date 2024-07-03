using Hymson.MES.BackgroundServices.Tasks.Manufacture.TracingSourceSFC;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hymson.MES.BackgroundTasks.Rotor.Jobs.Manufacture
{
    /// <summary>
    /// 条码追溯任务
    /// </summary>
    [DisallowConcurrentExecution]
    internal class TracingSourceSFCJob : IJob
    {
        private readonly ILogger<TracingSourceSFCJob> _logger;
        private readonly ITracingSourceSFCService _tracingSourceSFCService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="tracingSourceSFCService"></param>
        public TracingSourceSFCJob(ILogger<TracingSourceSFCJob> logger,
            ITracingSourceSFCService tracingSourceSFCService)
        {
            _logger = logger;
            _tracingSourceSFCService = tracingSourceSFCService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {

                await _tracingSourceSFCService.ExecuteAsync(1000);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "条码追溯出错:");
            }
        }

    }
}
