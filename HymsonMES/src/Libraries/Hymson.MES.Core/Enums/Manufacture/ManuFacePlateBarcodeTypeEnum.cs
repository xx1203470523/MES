using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// 面板配置 条码类型
    /// </summary>
    public enum ManuFacePlateBarcodeTypeEnum
    {
        /// <summary>
        /// 产品序列码
        /// </summary>
        [Description("产品序列码")]
        Product = 0,
        /// <summary>
        /// 载具编码
        /// </summary>
        [Description("载具编码")]
        Vehicle = 1,
    }
}
