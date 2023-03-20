using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 工单状态 1：未开始；2：下达；3：生产中；4：完成；5：锁定；6：暂停中；
    /// </summary>
    public enum PlanWorkOrderStatusEnum : sbyte
    {
        /// <summary>
        /// 未开始
        /// </summary>
        [Description("未开始")]
        NotStarted = 1,
        /// <summary>
        /// 已下达
        /// </summary>
        [Description("已下达")]
        SendDown = 2,
        /// <summary>
        /// 生产中
        /// </summary>
        [Description("生产中")]
        InProduction = 3,
        /// <summary>
        /// 已完成
        /// </summary>
        [Description("已完成")]
        Finish = 4,
        /// <summary>
        /// 已关闭
        /// </summary>
        [Description("已关闭")]
        Closed = 5,
        /// <summary>
        /// 暂停中
        /// </summary>
        [Description("暂停中")]
        Pending = 6,
    }
}
