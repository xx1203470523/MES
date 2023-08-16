using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 枚举（消息状态）
    /// </summary>
    public enum MessageStatusEnum : sbyte
    {
        /// <summary>
        /// 触发
        /// </summary>
        [Description("触发")]
        Trigger = 1,
        /// <summary>
        /// 接收
        /// </summary>
        [Description("接收")]
        Receive = 2,
        /// <summary>
        /// 处理
        /// </summary>
        [Description("处理")]
        Handle = 3,
        /// <summary>
        /// 关闭
        /// </summary>
        [Description("关闭")]
        Close = 4
    }
}
