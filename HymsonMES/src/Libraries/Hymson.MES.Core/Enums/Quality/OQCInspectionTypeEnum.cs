using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Quality
{
    /// <summary>
    /// OQC检验类型枚举
    /// </summary>
    public enum OQCInspectionTypeEnum : sbyte
    {
        /// <summary>
        /// 常规校验
        /// </summary>
        [Description("常规校验")]
        Routine = 1,
        /// <summary>
        /// 外观检验
        /// </summary>
        [Description("外观检验")]
        Visual = 2,
        /// <summary>
        /// 包装检验
        /// </summary>
        [Description("包装检验")]
        Packaging = 3
    }
}
