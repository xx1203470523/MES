using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public enum EquOperationTypeEnum : sbyte
    {
        /// <summary>
        /// 注册
        /// </summary> 
        [Description("注册")]
        Register = 1,
        /// <summary>
        /// 入库
        /// </summary>
        [Description("入库")]
        Inbound = 2,
        /// <summary>
        /// 出库
        /// </summary>
        [Description("出库")]
        Outbound = 3,
        /// <summary>
        /// 绑定
        /// </summary>
        [Description("绑定")]
        Bind = 5,
        /// <summary>
        /// 绑定
        /// </summary>
        [Description("绑定")]
        Unbind = 6,
    }
}
