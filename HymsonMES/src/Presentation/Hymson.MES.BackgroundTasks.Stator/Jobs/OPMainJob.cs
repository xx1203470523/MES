﻿using Hymson.MES.BackgroundServices.Stator.Services;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Diagnostics;
using Hymson.Utils;

namespace Hymson.MES.BackgroundTasks.Stator
{
    /// <summary>
    /// 工序
    /// </summary>
    [DisallowConcurrentExecution]
    internal class OPMainJob : IJob
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        private readonly ILogger<OPMainJob> _logger;
        private readonly IOP010Service _op010Service;
        private readonly IOP020Service _op020Service;
        private readonly IOP030Service _op030Service;
        private readonly IOP050Service _op050Service;
        private readonly IOP060Service _op060Service;
        private readonly IOP070Service _op070Service;
        private readonly IOP080Service _op080Service;
        private readonly IOP090Service _op090Service;
        private readonly IOP100Service _op100Service;
        private readonly IOP120Service _op120Service;
        private readonly IOP160Service _op160Service;
        private readonly IOP190Service _op190Service;
        private readonly IOP210Service _op210Service;
        private readonly IOP340Service _op340Service;
        private readonly IOP490Service _op490Service;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="op010Service"></param>
        /// <param name="op020Service"></param>
        /// <param name="op030Service"></param>
        /// <param name="op050Service"></param>
        /// <param name="op060Service"></param>
        /// <param name="op070Service"></param>
        /// <param name="op080Service"></param>
        /// <param name="op090Service"></param>
        /// <param name="op100Service"></param>
        /// <param name="op120Service"></param>
        /// <param name="op160Service"></param>
        /// <param name="op190Service"></param>
        /// <param name="op210Service"></param>
        /// <param name="op340Service"></param>
        /// <param name="op490Service"></param>
        public OPMainJob(ILogger<OPMainJob> logger,
            IOP010Service op010Service,
            IOP020Service op020Service,
            IOP030Service op030Service,
            IOP050Service op050Service,
            IOP060Service op060Service,
            IOP070Service op070Service,
            IOP080Service op080Service,
            IOP090Service op090Service,
            IOP100Service op100Service,
            IOP120Service op120Service,
            IOP160Service op160Service,
            IOP190Service op190Service,
            IOP210Service op210Service,
            IOP340Service op340Service,
            IOP490Service op490Service)
        {
            _logger = logger;
            _op010Service = op010Service;
            _op020Service = op020Service;
            _op030Service = op030Service;
            _op050Service = op050Service;
            _op060Service = op060Service;
            _op070Service = op070Service;
            _op080Service = op080Service;
            _op090Service = op090Service;
            _op100Service = op100Service;
            _op120Service = op120Service;
            _op160Service = op160Service;
            _op190Service = op190Service;
            _op210Service = op210Service;
            _op340Service = op340Service;
            _op490Service = op490Service;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                // 顺序不要随意调整
                var mainLimit = 50;
                var defaultLimit = 50;

                _logger.LogInformation($"OP010 开始同步了；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");
                await LoopExecuteAsync(() => _op010Service.ExecuteAsync(mainLimit), "OP010");
                _logger.LogInformation($"OP010 同步完成；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");
                _logger.LogInformation($"OP020 开始同步了；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");
                await LoopExecuteAsync(() => _op020Service.ExecuteAsync(mainLimit), "OP020");
                _logger.LogInformation($"OP020 同步完成；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");
                //await LoopExecuteAsync(() => _op060Service.ExecuteAsync(defaultLimit), "OP060");
                _logger.LogInformation($"op070_1 开始同步了；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");
                await LoopExecuteAsync(() => _op070Service.ExecuteAsync(mainLimit, "op070_1"), "OP070");
                _logger.LogInformation($"op070_1 同步完成；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");

                _logger.LogInformation($"OP030 开始同步了；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");
                await LoopExecuteAsync(() => _op030Service.ExecuteAsync(defaultLimit), "OP030");
                _logger.LogInformation($"OP030 同步完成；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");
                //await LoopExecuteAsync(() => _op050Service.ExecuteAsync(defaultLimit), "OP050");
                _logger.LogInformation($"OP080 开始同步了；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");
                await LoopExecuteAsync(() => _op080Service.ExecuteAsync(mainLimit), "OP080");
                _logger.LogInformation($"OP080 同步完成；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");
                _logger.LogInformation($"OP090 开始同步了；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");
                await LoopExecuteAsync(() => _op090Service.ExecuteAsync(defaultLimit), "OP090");
                _logger.LogInformation($"OP090 同步完成；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");
                _logger.LogInformation($"OP100 开始同步了；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");
                await LoopExecuteAsync(() => _op100Service.ExecuteAsync(defaultLimit), "OP100");
                _logger.LogInformation($"OP100 同步完成；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");
                _logger.LogInformation($"OP120 开始同步了；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");
                await LoopExecuteAsync(() => _op120Service.ExecuteAsync(mainLimit), "OP120");
                _logger.LogInformation($"OP120 同步完成；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");
                _logger.LogInformation($"OP160 开始同步了；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");
                await LoopExecuteAsync(() => _op160Service.ExecuteAsync(mainLimit), "OP160");
                _logger.LogInformation($"OP160 同步完成；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");
                _logger.LogInformation($"OP190 开始同步了；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");
                await LoopExecuteAsync(() => _op190Service.ExecuteAsync(mainLimit), "OP190");
                _logger.LogInformation($"OP190 同步完成；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");
                _logger.LogInformation($"OP210 开始同步了；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");

                await LoopExecuteAsync(() => _op210Service.ExecuteAsync(mainLimit), "OP210");
                _logger.LogInformation($"OP210 同步完成；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");
                _logger.LogInformation($"OP340 开始同步了；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");
                await LoopExecuteAsync(() => _op340Service.ExecuteAsync(mainLimit), "OP340");
                _logger.LogInformation($"OP340 同步完成；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");
                _logger.LogInformation($"OP490 开始同步了；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");

                await LoopExecuteAsync(() => _op490Service.ExecuteAsync(mainLimit), "OP490");
                _logger.LogInformation($"OP490 同步完成；时间： {HymsonClock.Now().ToString("yyyyMMdd HH:mm:ss")}");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OPMainJob :");
            }
        }

        /// <summary>
        /// 循环执行
        /// </summary>
        /// <param name="asyncMethod"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private async Task LoopExecuteAsync(Func<Task<int>> asyncMethod, string name = "")
        {
            // 创建计时器实例
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (true)
            {
                var rows = await asyncMethod();
                if (rows <= 0) break;
            }

            stopwatch.Stop();
            _logger.LogDebug($"【{name}】执行完毕，耗时：{stopwatch.ElapsedMilliseconds}毫秒");
        }

    }
}
