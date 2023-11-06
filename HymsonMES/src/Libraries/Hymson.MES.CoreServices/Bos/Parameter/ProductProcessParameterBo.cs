using Hymson.MES.CoreServices.Bos.Manufacture;

namespace Hymson.MES.CoreServices.Bos.Parameter
{
    /// <summary>
    /// 产品过程参数（面板）
    /// </summary>
    public class ProductProcessParameterBo : ManufactureBo
    {
        /// <summary>
        /// 参数
        /// </summary>
        public IEnumerable<ProductParameterBo> Parameters { get; set; } = new List<ProductParameterBo>();
    }

}
