using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Plan
{
    /// <summary>
    /// 推送状态
    /// </summary>
    public enum PushStatusEnum : sbyte
    {
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
        Failure = 3
    }
}
