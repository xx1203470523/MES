using Hymson.MES.CoreServices.Services.Manufacture.ManuSfcSummary;

namespace Hymson.MES.BackgroundServices.Manufacture.Productionstatistic
{
    /// <summary>
    /// 
    /// </summary>
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
