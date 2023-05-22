using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// 产出绑定物料
    /// </summary>
    public enum ManuOutputBindMaterialTypeEnum : sbyte
    {
        /// <summary>
        /// 原材料
        /// </summary>
        [Description("原材料")]
        Feeding = 1,
        /// <summary>
        /// 产品 
        /// </summary>
        [Description("产品")]
        Product = 2
    }
}
