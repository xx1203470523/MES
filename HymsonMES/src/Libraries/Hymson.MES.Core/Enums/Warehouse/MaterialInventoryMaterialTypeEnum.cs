using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Warehouse
{
    /// <summary>
    /// 物料类型
    /// </summary>
    public enum MaterialInventoryMaterialTypeEnum : sbyte
    {
        /// <summary>
        /// 半成品
        /// </summary>
        [Description("半成品")]
        SelfMadeParts = 1,
        /// <summary>
        /// 采购件
        /// </summary>
        [Description("采购件")]
        PurchaseParts = 2,
        /// <summary>
        /// 成品
        /// </summary>
        [Description("成品")]
        FinishedParts = 3
    }
}
