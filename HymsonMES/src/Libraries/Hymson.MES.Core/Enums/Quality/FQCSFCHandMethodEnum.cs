using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Quality
{
    /// <summary>
    /// FQC检验单不合格处理方式枚举
    /// </summary>
    public enum FQCSFCHandMethodEnum : sbyte
    {
        /// <summary>
        /// 合格
        /// </summary>
        [Description("合格")]
        Qualified = 0,
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
        /// 返工
        /// </summary>
        [Description("返工")]
        Rework = 3,
        /// <summary>
        /// 报废
        /// </summary>
        [Description("报废")]
        Scrap = 4
    }

    /// <summary>
    /// 不合格挑选处置
    /// </summary>
    public enum FQCSFCHandMethodSelectEnum : sbyte
    {
        /// <summary>
        /// 合格
        /// </summary>
        [Description("合格")]
        Qualified = 0,
        /// <summary>
        /// 让步
        /// </summary>
        [Description("让步")]
        Concession = 1,
        /// <summary>
        /// 返工
        /// </summary>
        [Description("返工")]
        Rework = 3,
        /// <summary>
        /// 报废
        /// </summary>
        [Description("报废")]
        Scrap = 4
    }

}
