using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// 卸料类型（2：卸料剩余物料；3：卸料剩余物料并报废）
    /// </summary>
    public enum FeedingUnloadingTypeEnum : sbyte
    {
        /// <summary>
        /// 卸料剩余物料
        /// </summary>
        [Description("卸料剩余物料")]
        UnLoad = 2,
        /// <summary>
        /// 卸料剩余物料并报废
        /// </summary>
        [Description("卸料剩余物料并报废")]
        UnLoadAndAbandon = 3
    }
}
