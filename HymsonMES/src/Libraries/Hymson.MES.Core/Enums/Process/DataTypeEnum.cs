using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 数据类型枚举
    /// </summary>
    public enum DataTypeEnum : sbyte
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
        /// <summary>
        /// 公式
        /// </summary>
        [Description("公式")]
        Equation = 3
    }
}
