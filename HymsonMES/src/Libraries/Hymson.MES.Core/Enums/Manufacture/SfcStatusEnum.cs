using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 条码状态枚举
    /// </summary>
    public enum SfcStatusEnum : sbyte
    {
        /// <summary>
        /// 排队中
        /// </summary>
        [Description("排队中")]
        lineUp = 1,
        /// <summary>
        /// 活动中
        /// </summary>
        [Description("活动中")]
        Activity = 2,
        /// <summary>
        /// 完成
        /// </summary>
        [Description("完成")]
        InProductionComplete = 3,
        /// <summary>
        /// 已完成
        /// </summary>
        [Description("已完成")]
        Complete = 4,
        /// <summary>
        /// 锁定
        /// </summary>
        [Description("锁定")]
        Locked = 5,
        /// <summary>
        /// 报废
        /// </summary>
        [Description("报废")]
        Scrapping = 6,
        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        Delete = 7,
        /// <summary>
        /// 无效
        /// </summary>
        [Description("无效")]
        Invalid = 8,
        /// <summary>
        /// 离脱
        /// </summary>
        [Description("离脱")]
        Detachment = 9
    }
}
