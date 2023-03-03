using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 物料来源枚举
    /// </summary>
    public enum MaterialOriginEnum : sbyte
    {
        /// <summary>
        /// 手工录入
        /// </summary>
        [Description("手工录入")]
        ManualEntry = 1,
        /// <summary>
        /// ERP
        /// </summary>
        [Description("ERP")]
        ERP = 2
    }
}
