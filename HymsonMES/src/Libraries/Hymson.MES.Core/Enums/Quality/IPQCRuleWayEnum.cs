using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Quality
{
    /// <summary>
    /// IQPC检验项目-检验规则-检验方式
    /// </summary>
    public enum IPQCRuleWayEnum : sbyte
    {
        /// <summary>
        /// 停机
        /// </summary>
        [Description("停机")]
        Stop = 1,
        /// <summary>
        /// 不停机
        /// </summary>
        [Description("不停机")]
        NonStop = 2
    }
}
