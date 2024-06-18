using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Equipment
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum BindOperationTypeEnum : sbyte
    {
        /// <summary>
        /// 安装
        /// </summary>
        [Description("安装")]
        Install = 1,

        /// <summary>
        /// 卸载
        /// </summary>
        [Description("卸载")]
        Uninstall = 2
    }
}
