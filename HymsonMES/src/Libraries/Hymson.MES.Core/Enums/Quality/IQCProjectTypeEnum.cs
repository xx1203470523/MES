using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Quality
{
    /// <summary>
    /// IQC项目类型;1、计量，2、计数
    /// </summary>
    public enum IQCProjectTypeEnum : sbyte
    {
        /// <summary>
        /// 计量
        /// </summary>
        [Description("计量")]
        Measure = 1,
        /// <summary>
        /// 计数
        /// </summary>
        [Description("计数")]
        Count = 2
    }
}
