using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Quality
{
    /// <summary>
    /// IQPC检验项目-检验规则-单位
    /// </summary>
    public enum IPQCRuleUnitEnum : sbyte
    {
        /// <summary>
        /// 台
        /// </summary>
        [Description("台")]
        An = 1,
        /// <summary>
        /// %
        /// </summary>
        [Description("%")]
        Percent = 2
    }
}
