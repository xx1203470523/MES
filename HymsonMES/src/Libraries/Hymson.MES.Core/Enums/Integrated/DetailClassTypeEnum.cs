using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Integrated
{
    /// <summary>
    /// 班次类型
    /// </summary>
    public enum DetailClassTypeEnum : sbyte
    {
        /// <summary>
        /// 早班
        /// </summary>
        [Description("早班")]
        Morning = 1,
        /// <summary>
        /// 中班
        /// </summary>
        [Description("中班")]
        Middle = 2,
        /// <summary>
        /// 晚班
        /// </summary>
        [Description("晚班")]
        Night = 3
    }
}
