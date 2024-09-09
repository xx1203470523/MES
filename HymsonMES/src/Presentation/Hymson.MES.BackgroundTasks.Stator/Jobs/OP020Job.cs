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
    internal class OP020Job : IJob
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        private readonly ILogger<OP020Job> _logger;
        private readonly IOP010Service _op010Service;
        private readonly IOP020Service _op020Service;
        private readonly IOP030Service _op030Service;
        private readonly IOP050Service _op050Service;
        private readonly IOP060Service _op060Service;
        private readonly IOP090Service _op090Service;
        private readonly IOP100Service _op100Service;
        private readonly IOP120Service _op120Service;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="op010Service"></param>
        /// <param name="op020Service"></param>
        /// <param name="op030Service"></param>
        /// <param name="op050Service"></param>
        /// <param name="op060Service"></param>
        /// <param name="op090Service"></param>
        /// <param name="op100Service"></param>
        /// <param name="op120Service"></param>
        public OP020Job(ILogger<OP020Job> logger,
            IOP010Service op010Service,
            IOP020Service op020Service,
            IOP030Service op030Service,
            IOP050Service op050Service,
            IOP060Service op060Service,
            IOP090Service op090Service,
            IOP100Service op100Service,
            IOP120Service op120Service)
        {
            _logger = logger;
            _op010Service = op010Service;
            _op020Service = op020Service;
            _op030Service = op030Service;
            _op050Service = op050Service;
            _op060Service = op060Service;
            _op090Service = op090Service;
            _op100Service = op100Service;
            _op120Service = op120Service;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogDebug($"【OP020Job】启动");

            // 创建计时器实例
            Stopwatch stopwatch = new();
            stopwatch.Start();

            try
            {
                var rows = 0;
                await _op010Service.ExecuteAsync(StatorConst.MAXLIMIT);
                rows = await _op020Service.ExecuteAsync(StatorConst.MAXLIMIT);
                await _op060Service.ExecuteAsync(100); // 参数太多，容易报错

                // 顺序不要随意调整
                await _op030Service.ExecuteAsync(StatorConst.MAXLIMIT);
                await _op050Service.ExecuteAsync(StatorConst.MAXLIMIT);
                await _op090Service.ExecuteAsync(StatorConst.MAXLIMIT);
                await _op100Service.ExecuteAsync(StatorConst.MAXLIMIT);
                await _op120Service.ExecuteAsync(StatorConst.MAXLIMIT);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OP020Job :");
            }

            stopwatch.Stop();
            _logger.LogDebug($"【OP020Job】执行完毕，耗时：{stopwatch.ElapsedMilliseconds}毫秒");
        }

    }
}
