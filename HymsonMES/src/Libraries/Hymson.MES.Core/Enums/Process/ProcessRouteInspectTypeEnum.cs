using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Process
{
    /// <summary>
    /// 工艺抽检类型
    /// </summary>
    public enum ProcessRouteInspectTypeEnum : sbyte
    {
        /// <summary>
        /// 空值
        /// </summary>
        [Description("空值")]
        None = 1,
        /// <summary>
        /// 固定比例
        /// </summary>
        [Description("固定比例")]
        FixedScale = 2,
        /// <summary>
        /// 随机抽检
        /// </summary>
        [Description("随机抽检")]
        RandomInspection = 3,
        /// <summary>
        /// 特殊抽检
        /// </summary>
        [Description("特殊抽检")]
        SpecialSamplingInspection = 4
    }
}
