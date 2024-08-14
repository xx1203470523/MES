using Hymson.MES.BackgroundServices.NIO.Services.ERP;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundTasks.NIO.Jobs.ERP
{
    /// <summary>
    /// 关键下级键
    /// </summary>
    [DisallowConcurrentExecution]
    internal class KeySubordinateJob : IJob
    {
        private readonly ILogger<KeySubordinateJob> _logger;
        private readonly IErpDataPushService _erpDataPushService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="buzDataPushService"></param>
        public KeySubordinateJob(ILogger<KeySubordinateJob> logger, IErpDataPushService erpDataPushService)
        {
            _logger = logger;
            _erpDataPushService = erpDataPushService;
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
                await _erpDataPushService.NioKeyItemInfoAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "扫描推送数据 -> 关键下级键");
            }
        }
    }
}
