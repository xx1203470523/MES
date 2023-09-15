using Hymson.MessagePush.Services;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundTasks.Jobs
{
    [DisallowConcurrentExecution]
    internal class MessagePushJob : IJob
    {
        private readonly IMessageService _messageService;
        private readonly ILogger<MessagePushJob> _logger;

        public MessagePushJob(IMessageService messageService,ILogger<MessagePushJob> logger)
        {
            this._messageService = messageService;
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            if (context.CancellationToken.IsCancellationRequested) return;
            try
            {
                await _messageService.SendMessageAsync("MES.PushMessage.BackGroundService");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex,"定时执行发送消息出错:");
            }
        }
    }
}
