using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Quality
{
    /// <summary>
    /// 检验类型
    /// </summary>
    public enum IQCInspectionTypeEnum
    {
        /// <summary>
        /// 常规检验
        /// </summary>
        [Description("常规检验")]
        RoutineInspection = 1,

        /// <summary>
        /// 外观检验
        /// </summary>
        [Description("外观检验")]
        InspectionOfAppearance = 2,

        /// <summary>
        /// 包装检验
        /// </summary>
        [Description("包装检验")]
        InspectionOfPacking = 3,

        /// <summary>
        /// 特殊性检验
        /// </summary>
        [Description("特殊性检验")]
        SpecificityTest = 4,

        /// <summary>
        /// 破坏性检验
        /// </summary>
        [Description("破坏性检验")]
        DestructiveInspection = 5
    }
}
