using Hymson.MES.BackgroundServices.NIO.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hymson.MES.BackgroundTasks.NIO
{
    /// <summary>
    /// 主数据（控制项）
    /// </summary>
    [DisallowConcurrentExecution]
    internal class MasterFieldJob : IJob
    {
        private readonly ILogger<MasterFieldJob> _logger;
        private readonly IMasterDataPushService _masterDataPushService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="masterDataPushService"></param>
        public MasterFieldJob(ILogger<MasterFieldJob> logger, IMasterDataPushService masterDataPushService)
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
                await _masterDataPushService.FieldAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "推送 -> 主数据（控制项）:");
            }
        }

    }
}
