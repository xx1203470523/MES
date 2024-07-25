using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Plan
{
    /// <summary>
    /// 推送状态
    /// </summary>
    public enum PushStatusEnum : sbyte
    {
        /// <summary>
        /// 无需推送
        /// </summary>
        [Description("无需推送")]
        NoNeed = 0,
        /// <summary>
        /// 待推送
        /// </summary>
        [Description("待推送")]
        Wait = 1,
        /// <summary>
        /// 已推送 
        /// </summary> 
        [Description("已推送")]
        Success = 2,
        /// <summary>
        /// 推送失败 
        /// </summary> 
        [Description("推送失败")]
        Failure = 3,
        /// <summary>
        /// 没有配置 
        /// </summary> 
        [Description("没有配置")]
        NotConfigured = 4,
        /// <summary>
        /// 推送开关关闭 
        /// </summary> 
        [Description("推送开关关闭")]
        Off = 5
    }
}
