using Hymson.MES.BackgroundServices.Stator;
using Hymson.MES.BackgroundServices.Stator.Services;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Diagnostics;

namespace Hymson.MES.BackgroundTasks.Stator
{
    /// <summary>
    /// 工序
    /// </summary>
    [DisallowConcurrentExecution]
    internal class OP070Job : IJob
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        private readonly ILogger<OP070Job> _logger;
        private readonly IOP070Service _op070Service;
        private readonly IOP130Service _op130Service;
        private readonly IOP150Service _op150Service;
        private readonly IOP170Service _op170Service;
        private readonly IOP180Service _op180Service;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="op070Service"></param>
        /// <param name="op130Service"></param>
        /// <param name="op150Service"></param>
        /// <param name="op170Service"></param>
        /// <param name="op180Service"></param>
        public OP070Job(ILogger<OP070Job> logger,
            IOP070Service op070Service,
            IOP130Service op130Service,
            IOP150Service op150Service,
            IOP170Service op170Service,
            IOP180Service op180Service)
        {
            _logger = logger;
            _op070Service = op070Service;
            _op130Service = op130Service;
            _op150Service = op150Service;
            _op170Service = op170Service;
            _op180Service = op180Service;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogDebug($"【OP070Job】启动");

            // 创建计时器实例
            Stopwatch stopwatch = new();
            stopwatch.Start();

            try
            {
                var rows = 0;
                rows = await _op070Service.ExecuteAsync(StatorConst.MAXLIMIT, "op070_1");

                // 顺序不要随意调整
                await _op130Service.ExecuteAsync(StatorConst.MAXLIMIT);
                await _op150Service.ExecuteAsync(StatorConst.MAXLIMIT);
                await _op170Service.ExecuteAsync(StatorConst.MAXLIMIT);
                await _op180Service.ExecuteAsync(StatorConst.MAXLIMIT);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OP070Job :");
            }

            stopwatch.Stop();
            _logger.LogDebug($"【OP070Job】执行完毕，耗时：{stopwatch.ElapsedMilliseconds}毫秒");
        }

    }
}
