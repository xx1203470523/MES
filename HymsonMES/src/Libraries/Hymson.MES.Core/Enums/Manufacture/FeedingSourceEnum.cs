using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// 物料来源
    /// </summary>
    public enum FeedingSourceEnum : sbyte
    {
        /// <summary>
        /// 设备
        /// </summary>
        [Description("设备")]
        Equipment = 1,
        /// <summary>
        /// 资源
        /// </summary>
        [Description("资源")]
        Resource = 2
    }
}
