using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Equipment.EquMaintenance
{
    public enum EquMaintenanceDataTypeEnum:sbyte
    {
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本")]
        Text = 1,
        /// <summary>
        /// 数值
        /// </summary>
        [Description("数值")]
        Numeric = 2,
    }
}
