namespace Hymson.MES.SignalRHub.Hubs
{
    public interface IChatClient
    {
        /// <summary>
        /// SignalR接收信息
        /// </summary>
        /// <param name="message">信息内容</param>
        /// <returns></returns>
        Task ReceiveMessage(object message);
    }
}
