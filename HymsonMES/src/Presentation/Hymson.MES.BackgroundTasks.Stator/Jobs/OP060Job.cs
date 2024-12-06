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
    internal class OP060Job : IJob
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        private readonly ILogger<OP060Job> _logger;
        private readonly IOP060Service _op060Service;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="op060Service"></param>
        public OP060Job(ILogger<OP060Job> logger,
            IOP060Service op060Service)
        {
            _logger = logger;
            _op060Service = op060Service;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            // 创建计时器实例
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                await Task.CompletedTask;
                //await _op060Service.ExecuteAsync(50);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OP060Job :");
            }

            stopwatch.Stop();
            _logger.LogDebug($"【{typeof(OP060).Name}】执行完毕，耗时：{stopwatch.ElapsedMilliseconds}毫秒");
        }

    }
}
