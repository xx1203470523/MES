using Hymson.MES.BackgroundServices.Quality.EnvOrderCreate;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hymson.MES.BackgroundTasks.Quality
{
    /// <summary>
    /// 环境检验单自动生成
    /// </summary>
    [DisallowConcurrentExecution]
    internal class EnvOrderCreateJob : IJob
    {
        private readonly ILogger<EnvOrderCreateJob> _logger;
        private readonly IEnvOrderAutoCreateService _envOrderAutoCreateService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="envOrderAutoCreateService"></param>
        public EnvOrderCreateJob(ILogger<EnvOrderCreateJob> logger, IEnvOrderAutoCreateService envOrderAutoCreateService)
        {
            _logger = logger;
            _envOrderAutoCreateService = envOrderAutoCreateService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _envOrderAutoCreateService.ExecuteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "环境检验单自动生成出错:");
            }
        }
    }
}
