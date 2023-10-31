using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// manu_sfc_info条码状态枚举
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
        /// 完成-在制
        /// </summary>
        [Description("完成-在制")]
        InProductionComplete = 3,

        /// <summary>
        /// 完成
        /// </summary>
        [Description("完成")]
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
        Delete = 7
    }
}
