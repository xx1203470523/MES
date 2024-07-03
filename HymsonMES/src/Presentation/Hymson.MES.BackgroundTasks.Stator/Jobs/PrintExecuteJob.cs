using Hymson.Print.Abstractions;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hymson.MES.BackgroundTasks.Stator.Jobs
{
    [DisallowConcurrentExecution]
    internal class PrintExecuteJob : IJob
    {
        private readonly IPrintBackgroundService _printBackgroundService;
        private readonly ILogger<PrintExecuteJob> _logger;

        public PrintExecuteJob(IPrintBackgroundService printBackgroundService, ILogger<PrintExecuteJob> logger)
        {
            _printBackgroundService = printBackgroundService;
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _printBackgroundService.BackgroundExecuteAsync(10, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "后台执行打印出错:");
            }
        }
    }
}
