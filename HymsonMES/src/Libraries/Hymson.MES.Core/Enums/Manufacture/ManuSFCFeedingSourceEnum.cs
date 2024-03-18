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

    #region 顷刻

    /// <summary>
    /// 上料转移类型
    /// </summary>
    public enum ManuSFCFeedingTransferEnum : sbyte
    {
        /// <summary>
        /// 资源间转移
        /// </summary>
        Resource = 1,
        /// <summary>
        /// 上料点之间转移
        /// </summary>
        FeedingPoint = 2,
        /// <summary>
        /// 资源到上料点转移
        /// </summary>
        ResourceFeedingPoint = 3,
        /// <summary>
        /// 上料点到资源转移
        /// </summary>
        FeedingPointResource = 4
    }

    #endregion
}
