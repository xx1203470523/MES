using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Parameter;

namespace Hymson.MES.EquipmentServices.Dtos.Parameter
{
    /// <summary>
    /// 产品过程参数
    /// </summary>
    public record ProductProcessParameterDto : BaseDto
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

}
