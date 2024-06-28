namespace Hymson.MES.SignalRHub.Hubs
{
    public class ClientMessageDto
    {
        /// <summary>
        /// 客户端方法
        /// </summary>
        public string ClientEventName { set; get; }

        /// <summary>
        /// 发送数据
        /// </summary>
        public object Data { set; get; }
    }
}
