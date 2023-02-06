using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundTasks.Jobs
{
    internal class HelloWorld2Job : IJob
    {
        private readonly ILogger<HelloWorld2Job> _logger;

        public HelloWorld2Job(ILogger<HelloWorld2Job> logger)
        {
            _logger = logger;
        }
        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("HelloWorld2Job Execute");
            return Task.CompletedTask;
        }
    }
}
