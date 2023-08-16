using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// manu_sfc_produce 条码生产状态:排队、活动、完成、NC 
    /// </summary>
    public enum SfcProduceStatusEnum : sbyte
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
        Complete = 3,

        /// <summary>
        /// 锁定
        /// </summary>
        [Description("锁定")]
        Locked = 4
    }

    public enum SfcProduceStatusDisplayEnum : sbyte
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
        /// 锁定
        /// </summary>
        [Description("锁定")]
        Locked = 4
    }

}
