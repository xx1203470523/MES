using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 点检任务类型
    /// </summary>
    public enum EquInspectionTypeEnum : sbyte
    {
        /// <summary>
        /// 日点检
        /// </summary>
        [Description("日点检")]
        DailyInspection = 1,
        /// <summary>
        /// 周点检
        /// </summary>
        [Description("周点检")]
        WeeklyInspection = 2
    }
}
