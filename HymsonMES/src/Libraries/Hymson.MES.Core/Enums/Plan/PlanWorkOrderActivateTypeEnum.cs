using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 工单激活类型
    /// </summary>
    public enum PlanWorkOrderActivateTypeEnum : sbyte
    {
        /// <summary>
        /// 激活
        /// </summary>
        [Description("激活")]
        Activate = 1,
        /// <summary>
        /// 取消激活
        /// </summary>
        [Description("取消激活")]
        CancelActivate = 2
    }
}
