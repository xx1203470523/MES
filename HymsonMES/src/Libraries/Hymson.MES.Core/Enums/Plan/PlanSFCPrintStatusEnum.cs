using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 条码打印状态
    /// </summary>
    public enum PlanSFCPrintStatusEnum : sbyte
    {
        /// <summary>
        /// 未打印 
        /// </summary> 
        [Description("未打印")]
        NotPrint = 0,
        /// <summary>
        /// 已打印
        /// </summary>
        [Description("已打印")]
        Print = 1,
    }
}
