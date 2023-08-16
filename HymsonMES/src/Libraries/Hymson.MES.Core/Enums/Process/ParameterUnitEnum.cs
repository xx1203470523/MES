using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 参数单位枚举
    /// </summary>
    public enum ParameterUnitEnum : sbyte
    {
        /// <summary>
        /// 个
        /// </summary>
        [Description("个")]
        Ge = 1,
        /// <summary>
        /// 只
        /// </summary>
        [Description("只")]
        Zhi = 2,
        /// <summary>
        /// 斤
        /// </summary>
        [Description("斤")]
        Jin = 3,
    }
}
