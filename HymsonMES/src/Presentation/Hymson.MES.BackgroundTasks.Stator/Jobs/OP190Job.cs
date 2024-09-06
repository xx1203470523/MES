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
    internal class OP190Job : IJob
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        private readonly ILogger<OP190Job> _logger;
        private readonly IOP190Service _op190Service;
        private readonly IOP200Service _op200Service;
        private readonly IOP210Service _op210Service;

        /// <summary>
        /// 工序接口
        /// </summary>
        private readonly IOP240Service _op240Service;
        private readonly IOP260Service _op260Service;
        private readonly IOP290Service _op290Service;
        private readonly IOP310Service _op310Service;
        private readonly IOP350Service _op350Service;
        private readonly IOP390Service _op390Service;
        private readonly IOP410Service _op410Service;
        private readonly IOP415Service _op415Service;
        private readonly IOP420Service _op420Service;
        private readonly IOP430Service _op430Service;
        private readonly IOP450Service _op450Service;
        private readonly IOP455Service _op455Service;
        private readonly IOP470Service _op470Service;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="op190Service"></param>
        /// <param name="op200Service"></param>
        /// <param name="op210Service"></param>
        public OP190Job(ILogger<OP190Job> logger,
            IOP190Service op190Service,
            IOP200Service op200Service,
            IOP210Service op210Service,

            IOP240Service op240Service,
            IOP260Service op260Service,
            IOP290Service op290Service,
            IOP310Service op310Service,
            IOP350Service op350Service,
            IOP390Service op390Service,
            IOP410Service op410Service,
            IOP415Service op415Service,
            IOP420Service op420Service,
            IOP430Service op430Service,
            IOP450Service op450Service,
            IOP455Service op455Service,
            IOP470Service op470Service)
        {
            _logger = logger;
            _op190Service = op190Service;
            _op200Service = op200Service;
            _op210Service = op210Service;

            _op240Service = op240Service;
            _op260Service = op260Service;
            _op290Service = op290Service;
            _op310Service = op310Service;
            _op350Service = op350Service;
            _op390Service = op390Service;
            _op410Service = op410Service;
            _op415Service = op415Service;
            _op420Service = op420Service;
            _op430Service = op430Service;
            _op450Service = op450Service;
            _op455Service = op455Service;
            _op470Service = op470Service;
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
                var rows = 0;
                // 这个OP210的记录好像比OP190的更早插入？
                rows = await _op210Service.ExecuteAsync(StatorConst.MAXLIMIT);
                await _op190Service.ExecuteAsync(StatorConst.MAXLIMIT);
                await _op200Service.ExecuteAsync(StatorConst.MAXLIMIT);

                if (rows > 0)
                {
                    // 顺序不要随意调整
                    await _op240Service.ExecuteAsync(StatorConst.MAXLIMIT);
                    await _op260Service.ExecuteAsync(StatorConst.MAXLIMIT);
                    await _op290Service.ExecuteAsync(StatorConst.MAXLIMIT);
                    await _op310Service.ExecuteAsync(StatorConst.MAXLIMIT);
                    await _op350Service.ExecuteAsync(StatorConst.MAXLIMIT);
                    await _op390Service.ExecuteAsync(StatorConst.MAXLIMIT);
                    await _op410Service.ExecuteAsync(StatorConst.MAXLIMIT);
                    await _op415Service.ExecuteAsync(StatorConst.MAXLIMIT);
                    await _op420Service.ExecuteAsync(StatorConst.MAXLIMIT);
                    await _op430Service.ExecuteAsync(StatorConst.MAXLIMIT);
                    await _op450Service.ExecuteAsync(StatorConst.MAXLIMIT);
                    await _op455Service.ExecuteAsync(StatorConst.MAXLIMIT);
                    await _op470Service.ExecuteAsync(StatorConst.MAXLIMIT);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OP190Job :");
            }

            stopwatch.Stop();
            _logger.LogDebug($"【OP190Job】执行完毕，耗时：{stopwatch.ElapsedMilliseconds}毫秒");
        }

    }
}
