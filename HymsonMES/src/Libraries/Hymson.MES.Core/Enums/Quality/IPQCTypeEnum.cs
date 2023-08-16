using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Quality
{
    /// <summary>
    /// IQPC检验项目类型
    /// </summary>
    public enum IPQCTypeEnum : sbyte
    {
        /// <summary>
        /// 首检
        /// </summary>
        [Description("首检")]
        FAI = 1,
        /// <summary>
        /// 巡检
        /// </summary>
        [Description("巡检")]
        IPQC = 3,
        /// <summary>
        /// 尾检
        /// </summary>
        [Description("尾检")]
        QTI = 2
    }
}
