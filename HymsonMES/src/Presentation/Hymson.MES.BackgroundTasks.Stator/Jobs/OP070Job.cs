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
        private readonly IOP080Service _op080Service;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="op070Service"></param>
        public OP070Job(ILogger<OP070Job> logger,
            IOP070Service op070Service,
            IOP080Service op080Service)
        {
            _logger = logger;
            _op070Service = op070Service;
            _op080Service = op080Service;
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
                /*
                await _op070Service.ExecuteAsync(300, "op070_1");
                await _op070Service.ExecuteAsync(300, "op070_2");
                await _op080Service.ExecuteAsync(200);
                */
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OP070Job :");
            }

            stopwatch.Stop();
            _logger.LogDebug($"【{typeof(OP070).Name}】执行完毕，耗时：{stopwatch.ElapsedMilliseconds}毫秒");
        }

    }
}
