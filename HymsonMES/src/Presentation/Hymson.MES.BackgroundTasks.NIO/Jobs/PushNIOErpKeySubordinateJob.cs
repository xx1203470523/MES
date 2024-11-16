using Hymson.MES.BackgroundServices.NIO.Services;
using Hymson.MES.Core.Enums.Mavel;
using Hymson.Utils;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hymson.MES.BackgroundTasks.NIO
{
    /// <summary>
    /// 推送蔚来作业
    /// </summary>
    [DisallowConcurrentExecution]
    internal class PushNIOErpKeySubordinateJob : IJob
    {
        private readonly ILogger<PushNIOErpKeySubordinateJob> _logger;
        private readonly IPushNIOService _pushNIOService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="pushNIOService"></param>
        public PushNIOErpKeySubordinateJob(ILogger<PushNIOErpKeySubordinateJob> logger, IPushNIOService pushNIOService)
        {
            _logger = logger;
            _pushNIOService = pushNIOService;
        }

        /// <summary>
        /// 执行:ERP（关键下级件信息）汇总 4020
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.LogInformation($"【PushNIOErpKeySubordinateJob】推送NIO的定时任务 -> 入口；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");
                await _pushNIOService.ExecutePushByBuzSceneAsync(BuzSceneEnum.ERP_KeySubordinate_Summary, 250);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "【PushNIOErpKeySubordinateJob】推送 -> NIO:");
            }
        }

    }
}
