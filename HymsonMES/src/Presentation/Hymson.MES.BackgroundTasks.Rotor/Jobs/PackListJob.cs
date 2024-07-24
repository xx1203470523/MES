using Hymson.MES.BackgroundServices.Rotor.Services;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundTasks.Rotor.Jobs
{
    [DisallowConcurrentExecution]
    internal class PackListJob : IJob
    {
        private readonly IPackListService _packListService;
        private readonly ILogger<PackListJob> _logger;

        public PackListJob(IPackListService packListService, ILogger<PackListJob> logger)
        {
            _packListService = packListService;
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _packListService.ExecAsync(50);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "后台执行sql语句出错:");
            }
        }
    }
}
