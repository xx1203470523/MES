using Hymson.MessagePush.Services;
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

        public MessagePushJob(IMessageService messageService)
        {
            this._messageService = messageService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            if (context.CancellationToken.IsCancellationRequested) return;
            await _messageService.SendMessageAsync("MES.PushMessage.BackGroundService");
        }
    }
}
