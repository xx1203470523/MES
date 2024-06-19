using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public enum EquRepairOrderStatusEnum : sbyte 
    {
        /// <summary>
        /// 待维修
        /// </summary>
        [Description("待维修")]
        PendingRepair = 1,
        /// <summary>
        /// 已维修
        /// </summary>
        [Description("已维修")]
        Repaired = 2,
        /// <summary>
        /// 已确认
        /// </summary>
        [Description("已确认")]
        Confirmed = 3, 
    }
}
