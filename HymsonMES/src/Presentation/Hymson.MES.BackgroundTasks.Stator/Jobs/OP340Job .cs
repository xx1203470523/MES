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
    internal class OP340Job : IJob
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        private readonly ILogger<OP340Job> _logger;
        private readonly IOP340Service _op340Service;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="op340Service"></param>
        public OP340Job(ILogger<OP340Job> logger,
            IOP340Service op340Service)
        {
            _logger = logger;
            _op340Service = op340Service;
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
                await _op340Service.ExecuteAsync(StatorConst.MAXLIMIT);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OP340Job  :");
            }

            stopwatch.Stop();
            _logger.LogDebug($"【OP340Job】执行完毕，耗时：{stopwatch.ElapsedMilliseconds}毫秒");
        }

    }
}
