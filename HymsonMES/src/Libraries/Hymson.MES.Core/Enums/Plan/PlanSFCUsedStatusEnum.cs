using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 条码使用状态（这个类型前端有用到，对应 YesOrNoEnum）
    /// </summary>
    public enum PlanSFCUsedStatusEnum : sbyte
    {
        /// <summary>
        /// 已使用
        /// </summary>
        [Description("已使用")]
        Used = 1,
        /// <summary>
        /// 未使用 
        /// </summary> 
        [Description("未使用")]
        NotUsed = 2
    }
}
