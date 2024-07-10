using Hymson.MES.BackgroundServices.NIO.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hymson.MES.BackgroundTasks.NIO
{
    /// <summary>
    /// 主数据（环境监测）
    /// </summary>
    [DisallowConcurrentExecution]
    internal class MasterEnvFieldJob : IJob
    {
        private readonly ILogger<MasterEnvFieldJob> _logger;
        private readonly IMasterDataPushService _masterDataPushService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="masterDataPushService"></param>
        public MasterEnvFieldJob(ILogger<MasterEnvFieldJob> logger, IMasterDataPushService masterDataPushService)
        {
            _logger = logger;
            _masterDataPushService = masterDataPushService;
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
                await _masterDataPushService.EnvFieldAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "推送 -> 主数据（环境监测）:");
            }
        }

    }
}
