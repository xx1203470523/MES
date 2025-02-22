﻿using Hymson.Logging.Services;
using Hymson.MES.BackgroundServices.Tasks.Manufacture.WorkOrderStatistic;
using Quartz;

namespace Hymson.MES.BackgroundTasks.Jobs.Manufacture
{
    /// <summary>
    /// 工单统计任务
    /// </summary>
    [DisallowConcurrentExecution]
    internal class WorkOrderStatisticJob : IJob
    {
        private readonly IAlarmLogService _alarmLogService;
        private readonly IWorkOrderStatisticService _workOrderStatisticService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="alarmLogService"></param>
        /// <param name="workOrderStatisticService"></param>
        public WorkOrderStatisticJob(IAlarmLogService alarmLogService,
            IWorkOrderStatisticService workOrderStatisticService)
        {
            _alarmLogService = alarmLogService;
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
                _alarmLogService.WriteAlarmLogEntry(new Logging.AlarmLogEntry("工单统计出错:" + ex.Message, ex.StackTrace ?? ""));
            }
        }

    }
}
