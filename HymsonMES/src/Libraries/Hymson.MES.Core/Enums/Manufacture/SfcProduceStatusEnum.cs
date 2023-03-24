using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// manu_sfc_produce 条码生产状态:排队、活动、完成、NC 
    /// </summary>
    public enum SfcProduceStatusEnum : sbyte
    {
        /// <summary>
        /// 排队
        /// </summary>
        [Description("排队")]
        lineUp = 1,
        /// <summary>
        /// 活动
        /// </summary>
        [Description("活动")]
        Activity = 2
    }
}
