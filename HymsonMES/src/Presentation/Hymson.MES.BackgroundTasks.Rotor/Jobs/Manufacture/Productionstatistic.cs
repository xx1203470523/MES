using Hymson.MES.BackgroundServices.Tasks.Manufacture.Productionstatistic;
using Quartz;

namespace Hymson.MES.BackgroundTasks.Rotor.Jobs.Manufacture
{
    [DisallowConcurrentExecution]
    public class Productionstatistic : IJob
    {
        private readonly IProductionstatisticService _productionstatisticService;

        /// <summary>
        /// 生产统计
        /// </summary>
        /// <param name="manuSfcSummaryService"></param>
        public Productionstatistic(IProductionstatisticService productionstatisticService)
        {
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

                Console.WriteLine(ex);
            }
        }
    }
}
