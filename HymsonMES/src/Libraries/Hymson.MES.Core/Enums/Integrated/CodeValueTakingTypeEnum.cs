using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Integrated
{
    /// <summary>
    /// 编码规则组成-取值方式
    /// </summary>
    public enum CodeValueTakingTypeEnum : sbyte
    {
        /// <summary>
        /// 固定值
        /// </summary>
        [Description("固定值")]
        FixedValue = 1,
        /// <summary>
        /// 可变值
        /// </summary>
        [Description("可变值")]
        VariableValue = 2
    }
}
