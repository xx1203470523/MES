using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 物料采购类型枚举
    /// </summary>
    public enum MaterialBuyTypeEnum : sbyte
    {
        /// <summary>
        /// 自制
        /// </summary>
        [Description("自制")]
        SelfControl = 1,
        /// <summary>
        /// 采购
        /// </summary
        [Description("采购")]
        Purchase = 2,
        /// <summary>
        /// 自制/采购
        /// </summary>
        [Description("自制/采购")]
        SelfControlOrPurchase = 3
    }
}
