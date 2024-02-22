using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Quality
{
    /// <summary>
    /// 检验类型
    /// </summary>
    public enum InspectionTypeEnum : sbyte
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
        Packaging = 3,
        /// <summary>
        /// 特殊性检验
        /// </summary>
        [Description("特殊性检验")]
        Particularity = 4,
        /// <summary>
        /// 破坏性检验
        /// </summary>
        [Description("破坏性检验")]
        Destructive = 5
    }

}
