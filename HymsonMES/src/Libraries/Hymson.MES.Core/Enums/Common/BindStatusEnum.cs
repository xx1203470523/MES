using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Common
{
    /// <summary>
    /// 绑定状态
    /// </summary>
    public enum BindStatusEnum : sbyte
    {
        /// <summary>
        /// 解绑
        /// </summary>
        [Description("解绑")]
        UnBind = 0,
        /// <summary>
        /// 绑定
        /// </summary>
        [Description("绑定")]
        Bind = 1
    }
}
