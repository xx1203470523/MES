using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 物料加载来源
    /// </summary>
    public enum ManuSFCFeedingSourceEnum : sbyte
    {
        /// <summary>
        /// BOM
        /// </summary>
        [Description("BOM")]
        BOM = 1,
        /// <summary>
        /// 上料点
        /// </summary>
        [Description("上料点")]
        FeedingPoint = 2
    }
}
