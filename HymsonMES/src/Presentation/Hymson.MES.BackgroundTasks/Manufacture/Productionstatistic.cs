using Hymson.MES.BackgroundServices.Manufacture.Productionstatistic;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hymson.MES.BackgroundTasks.Manufacture
{
    [DisallowConcurrentExecution]
    public class Productionstatistic : IJob
    {
        private readonly ILogger<Productionstatistic> _logger;
        private readonly IProductionstatisticService _productionstatisticService;

        /// <summary>
        /// 生产统计
        /// </summary>
        /// <param name="manuSfcSummaryService"></param>
        public Productionstatistic(ILogger<Productionstatistic> logger, IProductionstatisticService productionstatisticService)
        {
            _logger = logger;
            _productionstatisticService = productionstatisticService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            if (context.CancellationToken.IsCancellationRequested) return;
            try
            {
                await _productionstatisticService.ExecuteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "生产统计出错:");
            }
        }
    }
}
