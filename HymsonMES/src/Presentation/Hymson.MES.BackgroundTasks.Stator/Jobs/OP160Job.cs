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
    internal class OP160Job : IJob
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        private readonly ILogger<OP160Job> _logger;
        private readonly IOP160Service _op160Service;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="op160Service"></param>
        public OP160Job(ILogger<OP160Job> logger,
            IOP160Service op160Service)
        {
            _logger = logger;
            _op160Service = op160Service;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            // 创建计时器实例
            Stopwatch stopwatch = new();
            stopwatch.Start();

            try
            {
                await _op160Service.ExecuteAsync(StatorConst.MAXLIMIT);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OP160Job :");
            }

            stopwatch.Stop();
            _logger.LogDebug($"【OP160Job】执行完毕，耗时：{stopwatch.ElapsedMilliseconds}毫秒");
        }

    }
}
