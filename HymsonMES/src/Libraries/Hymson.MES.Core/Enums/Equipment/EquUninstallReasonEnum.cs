using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 卸载原因
    /// </summary>
    public enum EquUninstallReasonEnum : sbyte
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 1,

        /// <summary>
        /// 异常
        /// </summary>
        [Description("异常")]
        Abnormal = 2
    }
}
