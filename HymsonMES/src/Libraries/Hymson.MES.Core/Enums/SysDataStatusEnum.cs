using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public enum SysDataStatusEnum : sbyte
    {
        /// <summary>
        /// 新建
        /// </summary>
        [Description("新建")]
        Build = 0,
        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")]
        Enable = 1,
        /// <summary>
        /// 保留
        /// </summary>
        [Description("保留")]
        Retain = 2,
        /// <summary>
        /// 废除
        /// </summary>
        [Description("废除")]
        Abolish = 3,
    }
}
