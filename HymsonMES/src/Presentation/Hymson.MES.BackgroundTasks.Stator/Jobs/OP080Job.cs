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
    internal class OP080Job : IJob
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        private readonly ILogger<OP080Job> _logger;
        private readonly IOP080Service _op080Service;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="op080Service"></param>
        public OP080Job(ILogger<OP080Job> logger,
            IOP080Service op080Service)
        {
            _logger = logger;
            _op080Service = op080Service;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogDebug($"【OP080Job】启动");

            // 创建计时器实例
            Stopwatch stopwatch = new();
            stopwatch.Start();

            try
            {
                await _op080Service.ExecuteAsync(StatorConst.MAXLIMIT);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OP080Job :");
            }

            stopwatch.Stop();
            _logger.LogDebug($"【OP080Job】执行完毕，耗时：{stopwatch.ElapsedMilliseconds}毫秒");
        }

    }
}
