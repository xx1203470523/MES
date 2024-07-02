using Hymson.MES.BackgroundServices.Manufacture;
using Hymson.Utils;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hymson.MES.BackgroundTasks.Manufacture
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
#if DM
                await _tracingSourceSFCService.ExecuteForDMAsync(1000);
#else
                await _tracingSourceSFCService.ExecuteAsync(100);
#endif
            }
            catch (Exception ex)
            {
                var date = HymsonClock.Now();
                _logger.LogError(ex, $"{date}条码追溯出错:");
            }
        }

    }
}
