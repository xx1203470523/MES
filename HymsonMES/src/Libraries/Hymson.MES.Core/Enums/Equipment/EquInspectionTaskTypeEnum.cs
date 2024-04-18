using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 点检任务类型
    /// </summary>
    public enum EquInspectionTaskTypeEnum : sbyte
    {
        /// <summary>
        /// 白班
        /// </summary>
        [Description("白班")]
        Day = 1,
        /// <summary>
        /// 晚班
        /// </summary>
        [Description("晚班")]
        Evening = 2,
        /// <summary>
        /// 巡检
        /// </summary>
        [Description("巡检")]
        Inspection = 3
    }
}
