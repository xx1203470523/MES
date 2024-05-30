using Hymson.MES.CoreServices.Services.Manufacture.ManuSfcSummary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.Tasks.Manufacture.Productionstatistic
{
    public class ProductionstatisticService : IProductionstatisticService
    {

        /// <summary>
        /// 统计服务
        /// </summary>
        private readonly IManuSfcSummaryService _manuSfcSummaryService;

        /// <summary>
        /// 
        /// </summary>
        public ProductionstatisticService(IManuSfcSummaryService manuSfcSummaryService)
        {
            _manuSfcSummaryService = manuSfcSummaryService;
        }

        /// <summary>
        /// 执行统计
        /// </summary>
        /// <returns></returns>
        public async Task ExecuteAsync()
        {
            await _manuSfcSummaryService.ExecutStatisticAsync("AUTO");
        }
    }
}
