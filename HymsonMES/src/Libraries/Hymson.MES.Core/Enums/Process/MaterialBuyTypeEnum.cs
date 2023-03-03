namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 物料来源枚举
    /// </summary>
    public enum MaterialBuyTypeEnum : sbyte
    {
        /// <summary>
        /// 自制
        /// </summary>
        SelfControl = 1,
        /// <summary>
        /// 采购
        /// </summary>
        Purchase = 2,
        /// <summary>
        /// 自制/采购
        /// </summary>
        SelfControlOrPurchase = 3
    }
}
