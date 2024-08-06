using Hymson.MES.BackgroundServices.Stator.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hymson.MES.BackgroundTasks.Stator
{
    /// <summary>
    /// 工序（op010）
    /// </summary>
    [DisallowConcurrentExecution]
    internal class OP010Job : IJob
    {
        private readonly ILogger<OP010Job> _logger;
        private readonly IOP010Service _op010Service;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="op010Service"></param>
        public OP010Job(ILogger<OP010Job> logger, IOP010Service op010Service)
        {
            _logger = logger;
            _op010Service = op010Service;
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
                _ = await _op010Service.ExecuteAsync(500);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OP010Job :");
            }
        }

    }
}
