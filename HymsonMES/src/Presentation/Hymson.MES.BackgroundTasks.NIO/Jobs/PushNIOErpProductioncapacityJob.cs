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
    internal class PushNIOErpProductioncapacityJob : IJob
    {
        private readonly ILogger<PushNIOErpProductioncapacityJob> _logger;
        private readonly IPushNIOService _pushNIOService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="pushNIOService"></param>
        public PushNIOErpProductioncapacityJob(ILogger<PushNIOErpProductioncapacityJob> logger, IPushNIOService pushNIOService)
        {
            _logger = logger;
            _pushNIOService = pushNIOService;
        }

        /// <summary>
        /// 执行：ERP（合作伙伴精益与生产能力）汇总 4010
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.LogInformation($"【PushNIOErpProductioncapacityJob】推送NIO的定时任务 -> 入口；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");
                await _pushNIOService.ExecutePushByBuzSceneAsync(BuzSceneEnum.ERP_ProductionCapacity_Summary, 250);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "【PushNIOErpProductioncapacityJob】推送 -> NIO:");
            }
        }

    }
}
