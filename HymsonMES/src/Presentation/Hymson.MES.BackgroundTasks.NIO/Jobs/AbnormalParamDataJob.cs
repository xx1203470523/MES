using Hymson.MES.BackgroundServices.NIO.Services;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundTasks.NIO.Jobs
{
    /// <summary>
    /// 异常参数处理JOB
    /// </summary>
    [DisallowConcurrentExecution]
    internal class AbnormalParamDataJob : IJob
    {
        private readonly ILogger<AbnormalParamDataJob> _logger;
        private readonly IAbnormalDataService _abnormalDataService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="abnormalDataService"></param>
        public AbnormalParamDataJob(ILogger<AbnormalParamDataJob> logger, IAbnormalDataService abnormalDataService)
        {
            _logger = logger;
            _abnormalDataService = abnormalDataService;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _abnormalDataService.RepeatParamAsync(7);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AbnormalParamDataJob -> NIO:");
            }
        }

    }
}
