﻿using Hymson.MES.BackgroundServices.NIO.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hymson.MES.BackgroundTasks.NIO
{
    /// <summary>
    /// 业务数据（产品一次合格率）
    /// </summary>
    [DisallowConcurrentExecution]
    internal class BuzPassrateProductJob : IJob
    {
        private readonly ILogger<BuzPassrateProductJob> _logger;
        private readonly IBuzDataPushService _buzDataPushService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="buzDataPushService"></param>
        public BuzPassrateProductJob(ILogger<BuzPassrateProductJob> logger, IBuzDataPushService buzDataPushService)
        {
            _logger = logger;
            _buzDataPushService = buzDataPushService;
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
                await _buzDataPushService.PassrateProductAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "推送 -> 业务数据（产品一次合格率）:");
            }
        }

    }
}