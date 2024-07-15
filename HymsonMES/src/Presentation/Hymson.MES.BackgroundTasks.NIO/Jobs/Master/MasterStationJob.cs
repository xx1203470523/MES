using Hymson.MES.BackgroundServices.NIO.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hymson.MES.BackgroundTasks.NIO
{
    /// <summary>
    /// 主数据（工站）
    /// </summary>
    [DisallowConcurrentExecution]
    internal class MasterStationJob : IJob
    {
        private readonly ILogger<MasterStationJob> _logger;
        private readonly IMasterDataPushService _masterDataPushService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="masterDataPushService"></param>
        public MasterStationJob(ILogger<MasterStationJob> logger, IMasterDataPushService masterDataPushService)
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
                await _masterDataPushService.StationAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "扫描推送数据 -> 主数据（工站）:");
            }
        }

    }
}
