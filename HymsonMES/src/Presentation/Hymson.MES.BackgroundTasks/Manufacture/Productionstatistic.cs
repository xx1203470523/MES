using Hymson.MES.BackgroundServices.Manufacture.Productionstatistic;
using Hymson.MES.CoreServices.Services.Manufacture.ManuSfcSummary;
using Hymson.MessagePush.Services;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundTasks.Manufacture
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
            this._productionstatisticService = productionstatisticService;
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
