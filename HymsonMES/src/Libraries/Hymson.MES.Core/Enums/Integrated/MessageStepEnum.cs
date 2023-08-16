using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 枚举（消息状态）
    /// </summary>
    public enum MessageStatusEnum : sbyte
    {
        // 这里的描述是要返回给前端，特意设置不一样的描述

        /// <summary>
        /// 触发
        /// </summary>
        [Description("待接收")]
        Trigger = 1,
        /// <summary>
        /// 接收
        /// </summary>
        [Description("待处理")]
        Receive = 2,
        /// <summary>
        /// 处理
        /// </summary>
        [Description("待关闭")]
        Handle = 3,
        /// <summary>
        /// 关闭
        /// </summary>
        [Description("关闭")]
        Close = 4
    }
}
