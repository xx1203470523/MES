using Hymson.SqlActuator.Services;
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
    internal class SqlExecuteJob : IJob
    {
        private readonly ISqlExecuteTaskService _sqlExecuteTaskService;
        private readonly ILogger<SqlExecuteJob> _logger;

        public SqlExecuteJob(ISqlExecuteTaskService sqlExecuteTaskService, ILogger<SqlExecuteJob> logger)
        {
            _sqlExecuteTaskService = sqlExecuteTaskService;
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _sqlExecuteTaskService.BackgroundExecuteAsync(100);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "后台执行sql语句出错:");
            }
        }
    }
}
