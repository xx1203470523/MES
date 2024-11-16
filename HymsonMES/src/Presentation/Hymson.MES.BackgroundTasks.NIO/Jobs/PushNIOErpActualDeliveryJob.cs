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
    internal class PushNIOErpActualDeliveryJob : IJob
    {
        private readonly ILogger<PushNIOErpActualDeliveryJob> _logger;
        private readonly IPushNIOService _pushNIOService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="pushNIOService"></param>
        public PushNIOErpActualDeliveryJob(ILogger<PushNIOErpActualDeliveryJob> logger, IPushNIOService pushNIOService)
        {
            _logger = logger;
            _pushNIOService = pushNIOService;
        }

        /// <summary>
        /// 执行：ERP（实际交付情况）汇总 4030
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.LogInformation($"【PushNIOErpActualDeliveryJob】推送NIO的定时任务 -> 入口；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");
                await _pushNIOService.ExecutePushByBuzSceneAsync(BuzSceneEnum.ERP_ActualDelivery_Summary, 250);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "【PushNIOErpActualDeliveryJob】推送 -> NIO:");
            }
        }

    }
}
