using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 条码使用状态（这个类型前端有用到）
    /// </summary>
    public enum PlanSFCUsedStatusEnum : sbyte
    {
        /// <summary>
        /// 未使用 
        /// </summary> 
        [Description("未使用")]
        NotUsed = 1,
        /// <summary>
        /// 已使用
        /// </summary>
        [Description("已使用")]
        Used = 2,
    }
}
