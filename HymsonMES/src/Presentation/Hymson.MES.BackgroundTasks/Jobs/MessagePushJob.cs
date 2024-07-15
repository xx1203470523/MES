using Hymson.Logging.Services;
using Hymson.MessagePush.Services;
using Quartz;

namespace Hymson.MES.BackgroundTasks.Jobs
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
        private readonly IAlarmLogService _alarmLogService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageService"></param>
        /// <param name="logger"></param>
        public MessagePushJob(IMessageService messageService, IAlarmLogService alarmLogService)
        {
            this._messageService = messageService;
            _alarmLogService = alarmLogService;
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
                _alarmLogService.WriteAlarmLogEntry(new Logging.AlarmLogEntry("定时执行发送消息出错:" + ex.Message, ex.StackTrace ?? ""));
            }
        }

    }
}
