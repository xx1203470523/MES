using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 物料库存来源类型
    /// </summary>
    public enum WhMaterialInventorySourceEnum : sbyte
    {
        /// <summary>
        /// 物料接收
        /// </summary>
        [Description("物料接收")]
        MaterialReceiving = 1,
        /// <summary>
        /// 物料退料
        /// </summary>
        [Description("物料退料")]
        MaterialReturn = 2,
        /// <summary>
        /// 物料加载
        /// </summary>
        [Description("物料加载")]
        MaterialLoading = 3
    }
}
