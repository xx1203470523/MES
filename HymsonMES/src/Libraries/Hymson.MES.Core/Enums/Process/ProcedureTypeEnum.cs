using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 工序类型
    /// </summary>
    public enum ProcedureTypeEnum : sbyte
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 1,
        /// <summary>
        /// 测试
        /// </summary>
        [Description("测试")]
        Test = 2,
        /// <summary>
        /// 包装
        /// </summary>
        [Description("包装")]
        Packing = 3
    }
}
