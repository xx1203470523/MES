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
    internal class OP490Job : IJob
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        private readonly ILogger<OP490Job> _logger;
        private readonly IOP490Service _op490Service;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="op490Service"></param>
        public OP490Job(ILogger<OP490Job> logger,
            IOP490Service op490Service)
        {
            _logger = logger;
            _op490Service = op490Service;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogDebug($"【OP490Job】启动");

            // 创建计时器实例
            Stopwatch stopwatch = new();
            stopwatch.Start();

            try
            {
                await _op490Service.ExecuteAsync(StatorConst.MAXLIMIT);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OP490Job  :");
            }

            stopwatch.Stop();
            _logger.LogDebug($"【OP490Job】执行完毕，耗时：{stopwatch.ElapsedMilliseconds}毫秒");
        }

    }
}
