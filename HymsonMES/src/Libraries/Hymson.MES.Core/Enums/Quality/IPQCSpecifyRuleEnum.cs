using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Quality
{
    /// <summary>
    /// IQPC检验项目-检验规则-指定规则
    /// </summary>
    public enum IPQCSpecifyRuleEnum : sbyte
    {
        /// <summary>
        /// 固定
        /// </summary>
        [Description("固定")]
        Fixed = 1,
        /// <summary>
        /// 随机
        /// </summary>
        [Description("随机")]
        Random = 2,
        /// <summary>
        /// 顺序
        /// </summary>
        [Description("顺序")]
        Orderly = 3
    }
}
