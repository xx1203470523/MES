using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 子步骤类型
    /// </summary>
    public enum ProcedureSubstepTypeEnum : sbyte
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 1,

        /// <summary>
        /// 可选
        /// </summary>
        [Description("可选")]
        Optional = 2
    }
}
