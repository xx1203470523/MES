using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 频率
    /// </summary>
    public enum FrequencyEnum : sbyte
    {
        /// <summary>
        /// 班次
        /// </summary>
        [Description("班次")]
        Classes = 12,
        /// <summary>
        /// 天
        /// </summary>
        [Description("天")]
        Day = 24,
        /// <summary>
        /// 1小时
        /// </summary>
        [Description("1小时")]
        HourOne = 1,
        /// <summary>
        /// 2小时
        /// </summary>
        [Description("2小时")]
        HourTwo = 2,
        /// <summary>
        /// 4小时
        /// </summary>
        [Description("4小时")]
        HourFour = 4,
        /// <summary>
        /// 8小时
        /// </summary>
        [Description("8小时")]
        HourEight = 8
    }
}
