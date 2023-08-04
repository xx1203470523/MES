using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 推送方式
    /// </summary>
    public enum PushTypeEnum : sbyte
    {
        /// <summary>
        /// 企业微信
        /// </summary>
        [Description("企业微信")]
        QiYeWechat = 1,
        /// <summary>
        /// 钉钉
        /// </summary>
        [Description("钉钉")]
        DingTalk = 2,
        /// <summary>
        /// 电子邮箱 
        /// </summary>
        [Description("电子邮箱")]
        Email = 3
    }
}
