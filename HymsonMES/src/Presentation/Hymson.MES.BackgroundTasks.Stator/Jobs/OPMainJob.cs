using Hymson.MES.BackgroundServices.Stator.Services;
using Microsoft.Extensions.Logging;
using Quartz;

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
        private readonly IOP070Service _op070Service;
        private readonly IOP120Service _op120Service;
        private readonly IOP190Service _op190Service;
        private readonly IOP210Service _op210Service;
        private readonly IOP340Service _op340Service;
        private readonly IOP490Service _op490Service;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="op010Service"></param>
        /// <param name="op070Service"></param>
        /// <param name="op120Service"></param>
        /// <param name="op190Service"></param>
        /// <param name="op210Service"></param>
        /// <param name="op340Service"></param>
        /// <param name="op490Service"></param>
        public OPMainJob(ILogger<OPMainJob> logger,
            IOP010Service op010Service,
            IOP070Service op070Service,
            IOP120Service op120Service,
            IOP190Service op190Service,
            IOP210Service op210Service,
            IOP340Service op340Service,
            IOP490Service op490Service)
        {
            _logger = logger;
            _op010Service = op010Service;
            _op070Service = op070Service;
            _op120Service = op120Service;
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
                _ = await _op070Service.ExecuteAsync(5000);
                _ = await _op120Service.ExecuteAsync(5000);
                _ = await _op010Service.ExecuteAsync(5000);
                _ = await _op190Service.ExecuteAsync(5000);
                _ = await _op210Service.ExecuteAsync(5000);
                _ = await _op340Service.ExecuteAsync(5000);
                _ = await _op490Service.ExecuteAsync(5000);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OPMainJob :");
            }
        }

    }
}
