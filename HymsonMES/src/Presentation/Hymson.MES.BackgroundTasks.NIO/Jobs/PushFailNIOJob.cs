using Hymson.MES.BackgroundServices.NIO.Services;
using Hymson.Utils;
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
    /// 推送已经失败蔚来作业
    /// </summary>
    [DisallowConcurrentExecution]
    internal class PushFailNIOJob : IJob
    {
        private readonly ILogger<PushFailNIOJob> _logger;
        private readonly IPushNIOService _pushNIOService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="pushNIOService"></param>
        public PushFailNIOJob(ILogger<PushFailNIOJob> logger, IPushNIOService pushNIOService)
        {
            _logger = logger;
            _pushNIOService = pushNIOService;
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
                _logger.LogInformation($"【PushFailNIOJob】推送NIO的定时任务 -> 入口；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");
                await _pushNIOService.ExecutePushFailAsync(20);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "【PushFailNIOJob】推送 -> NIO:");
            }
        }

    }
}
