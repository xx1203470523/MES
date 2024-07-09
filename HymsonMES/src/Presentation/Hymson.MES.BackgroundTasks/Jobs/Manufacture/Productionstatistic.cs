using Hymson.Logging.Services;
using Hymson.MES.BackgroundServices.Tasks.Manufacture.Productionstatistic;
using Quartz;

namespace Hymson.MES.BackgroundTasks.Jobs.Manufacture
{
    [DisallowConcurrentExecution]
    public class Productionstatistic : IJob
    {
        private readonly IProductionstatisticService _productionstatisticService;
        private readonly IAlarmLogService _alarmLogService;



        /// <summary>
        /// 生产统计
        /// </summary>
        /// <param name="productionstatisticService"></param>
        /// <param name="alarmLogService"></param>
        public Productionstatistic(IProductionstatisticService productionstatisticService, IAlarmLogService alarmLogService)
        {
            _productionstatisticService = productionstatisticService;
            _alarmLogService = alarmLogService;
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
                _alarmLogService.WriteAlarmLogEntry(new Logging.AlarmLogEntry(ex.Message, ex.StackTrace ?? ""));
            }
        }
    }
}
