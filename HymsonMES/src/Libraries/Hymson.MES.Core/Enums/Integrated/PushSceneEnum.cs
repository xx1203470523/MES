using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 枚举（推送场景）
    /// </summary>
    public enum PushSceneEnum : sbyte
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
        /// 接收升级
        /// </summary>
        [Description("接收升级")]
        ReceiveUpgrade = 3,
        /// <summary>
        /// 处理
        /// </summary>
        [Description("处理")]
        Handle = 4,
        /// <summary>
        /// 处理升级
        /// </summary>
        [Description("处理升级")]
        HandleUpgrade = 5,
        /// <summary>
        /// 关闭
        /// </summary>
        [Description("关闭")]
        Close = 6
    }
}
