using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Quality
{
    /// <summary>
    /// 来源系统枚举
    /// </summary>
    public enum SourceSystemEnum : sbyte
    {
        /// <summary>
        /// MES
        /// </summary>
        [Description("MES")]
        MES = 1,
        /// <summary>
        /// OA
        /// </summary>
        [Description("OA")]
        OA = 2
    }
}
