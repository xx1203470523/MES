using Hymson.MES.BackgroundServices.Tasks.Manufacture.WorkOrderStatistic;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hymson.MES.BackgroundTasks.Stator.Jobs.Manufacture
{
    /// <summary>
    /// 工单统计任务
    /// </summary>
    [DisallowConcurrentExecution]
    internal class WorkOrderStatisticJob : IJob
    {
        private readonly ILogger<WorkOrderStatisticJob> _logger;
        private readonly IWorkOrderStatisticService _workOrderStatisticService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="workOrderStatisticService"></param>
        public WorkOrderStatisticJob(ILogger<WorkOrderStatisticJob> logger,
            IWorkOrderStatisticService workOrderStatisticService)
        {
            _logger = logger;
            _workOrderStatisticService = workOrderStatisticService;
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
                await _workOrderStatisticService.ExecuteAsync(1000);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "工单统计出错:");
            }
        }

    }
}
