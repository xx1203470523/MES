using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundTasks.Jobs
{
    internal class HelloWorld1Job : IJob
    {
        private readonly ILogger<HelloWorld1Job> _logger;

        public HelloWorld1Job(ILogger<HelloWorld1Job> logger)
        {
            _logger = logger;
        }
        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("HelloWorld1Job Execute");
            return Task.CompletedTask;
        }
    }
}
