using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Integrated
{
    /// <summary>
    /// 时间通配-类型
    /// </summary>
    public enum TimeWildcardTypeEnum : sbyte
    {
        /// <summary>
        /// 年
        /// </summary>
        [Description("年")]
        Year = 1,
        /// <summary>
        /// 月
        /// </summary>
        [Description("月")]
        Month = 2,
        /// <summary>
        /// 日
        /// </summary>
        [Description("日")]
        Day = 3
    }
}
