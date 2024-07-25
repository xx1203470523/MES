using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Quality
{
    /// <summary>
    /// Fqc判定结果
    /// </summary>
    public enum FqcJudgmentResultsEnum : sbyte
    {
        /// <summary>
        /// 合格
        /// </summary>
        [Description("合格")]
        Qualified = 1,
        /// <summary>
        /// 不合格
        /// </summary>
        [Description("不合格")]
        Unqualified = 2,
    }
}
