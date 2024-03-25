using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Quality
{
    /// <summary>
    /// IQC不合格处理方式枚举
    /// </summary>
    public enum IQCHandMethodEnum
    {
        /// <summary>
        /// 让步
        /// </summary>
        [Description("让步")]
        Concession = 1,
        /// <summary>
        /// 挑选
        /// </summary>
        [Description("挑选")]
        Pick = 2,
        /// <summary>
        /// 退货
        /// </summary>
        [Description("退货")]
        Return = 3,
        /// <summary>
        /// 换货
        /// </summary>
        [Description("换货")]
        Replace = 4,
        /// <summary>
        /// 返工
        /// </summary>
        [Description("返工")]
        Rework = 5,
        /// <summary>
        /// 返修
        /// </summary>
        [Description("返修")]
        Repair = 6,
        /// <summary>
        /// 报废
        /// </summary>
        [Description("报废")]
        Scrap = 7
    }
}
