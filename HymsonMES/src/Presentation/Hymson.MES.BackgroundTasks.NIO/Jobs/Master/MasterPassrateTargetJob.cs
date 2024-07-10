using Hymson.MES.BackgroundServices.NIO.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hymson.MES.BackgroundTasks.NIO
{
    /// <summary>
    /// 主数据（一次合格率目标）
    /// </summary>
    [DisallowConcurrentExecution]
    internal class MasterPassrateTargetJob : IJob
    {
        private readonly ILogger<MasterPassrateTargetJob> _logger;
        private readonly IMasterDataPushService _masterDataPushService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="masterDataPushService"></param>
        public MasterPassrateTargetJob(ILogger<MasterPassrateTargetJob> logger, IMasterDataPushService masterDataPushService)
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
                await _masterDataPushService.PassrateTargetAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "推送 -> 主数据（一次合格率目标）:");
            }
        }

    }
}
