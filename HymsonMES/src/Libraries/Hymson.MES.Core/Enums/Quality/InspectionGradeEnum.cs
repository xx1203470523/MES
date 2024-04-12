using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Quality
{
    /// <summary>
    /// 检验等级
    /// </summary>
    public enum InspectionGradeEnum : sbyte
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 1,
        /// <summary>
        /// 加严
        /// </summary>
        [Description("加严")]
        Tighter = 2,
        /// <summary>
        /// 放宽
        /// </summary>
        [Description("放宽")]
        Relax = 3,
    }
}
