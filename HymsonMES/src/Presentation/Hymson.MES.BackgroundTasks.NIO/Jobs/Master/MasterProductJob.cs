using Hymson.MES.BackgroundServices.NIO.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hymson.MES.BackgroundTasks.NIO
{
    /// <summary>
    /// 主数据（产品）
    /// </summary>
    [DisallowConcurrentExecution]
    internal class MasterProductJob : IJob
    {
        private readonly ILogger<MasterProductJob> _logger;
        private readonly IMasterDataPushService _masterDataPushService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="masterDataPushService"></param>
        public MasterProductJob(ILogger<MasterProductJob> logger, IMasterDataPushService masterDataPushService)
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
                await _masterDataPushService.ProductAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "扫描推送数据 -> 主数据（产品）:");
            }
        }

    }
}
