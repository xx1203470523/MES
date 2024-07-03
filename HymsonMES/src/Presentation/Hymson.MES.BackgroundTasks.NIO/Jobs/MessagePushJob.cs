using Hymson.MessagePush.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hymson.MES.BackgroundTasks.NIO.Jobs
{
    /// <summary>
    /// 
    /// </summary>
    [DisallowConcurrentExecution]
    internal class MessagePushJob : IJob
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IMessageService _messageService;
        private readonly ILogger<MessagePushJob> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageService"></param>
        /// <param name="logger"></param>
        public MessagePushJob(IMessageService messageService, ILogger<MessagePushJob> logger)
        {
            _messageService = messageService;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            if (context.CancellationToken.IsCancellationRequested) return;
            try
            {
                await _messageService.SendMessageAsync("MES.PushMessage.BackGroundService");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "定时执行发送消息出错:");
            }
        }

    }
}
