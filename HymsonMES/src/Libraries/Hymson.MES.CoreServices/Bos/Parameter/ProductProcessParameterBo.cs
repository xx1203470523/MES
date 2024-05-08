using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture;

namespace Hymson.MES.CoreServices.Bos.Parameter
{
    /// <summary>
    /// 产品过程参数（面板）
    /// </summary>
    public record ProductProcessParameterBo : ManufactureBo
    {
        /// <summary>
        /// 条码类型
        /// </summary>
        public ManuFacePlateBarcodeTypeEnum Type { get; set; } = ManuFacePlateBarcodeTypeEnum.Product;

        /// <summary>
        /// 产品条码/载具编码
        /// </summary>
        public IEnumerable<string> SFCs { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public IEnumerable<ProductParameterBo> Parameters { get; set; } = new List<ProductParameterBo>();

    }

    /// <summary>
    /// 产品过程参数
    /// </summary>
    public record ProductParameterCollectBo : ManufactureBo
    {
        /// <summary>
        /// 参数列表
        /// </summary>
        public IEnumerable<ProductParameterCollectInfo> SFCList { get; set; } = new List<ProductParameterCollectInfo>();
    }

    /// <summary>
    /// 产品过程参数明细
    /// </summary>
    public record ProductParameterCollectInfo
    {
        /// <summary>
        /// 产品条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public IEnumerable<ProductParameterBo> Parameters { get; set; } = new List<ProductParameterBo>();
    }

}
