using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Common
{
    /// <summary>
    /// 数据实体（message_push）   
    /// message_push
    /// @author Czhipu
    /// @date 2023-08-19 11:02:16
    /// </summary>
    public class MessagePushEntity : BaseEntity
    {
        /// <summary>
        /// 发送目的地
        /// </summary>
        public string To { get; set; }

       /// <summary>
        /// 消息体
        /// </summary>
        public string Message { get; set; }

       /// <summary>
        /// 消息类型
        /// </summary>
        public bool MessageType { get; set; }

       /// <summary>
        /// 返回体数据包
        /// </summary>
        public string ResponseBody { get; set; }

       /// <summary>
        /// 发送状态
        /// </summary>
        public bool SendState { get; set; }

       
    }
}
