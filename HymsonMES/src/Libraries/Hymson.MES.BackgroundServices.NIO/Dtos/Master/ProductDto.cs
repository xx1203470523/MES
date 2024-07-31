namespace Hymson.MES.BackgroundServices.NIO.Dtos.Master
{
    /// <summary>
    /// 产品主数据
    /// </summary>
    public class ProductDto : BaseDto
    {
        /// <summary>
        /// 合作伙伴产品代码, max length 64
        /// </summary>
        public string VendorProductCode { get; set; }

        /// <summary>
        /// 合作伙伴产品名称，max length 64
        /// </summary>
        public string VendorProductName { get; set; }

        /// <summary>
        /// NIO产品料号, max length 64.
        /// </summary>
        public string NioProductCode { get; set; }

        /// <summary>
        /// NIO 产品名称, max length 64
        /// </summary>
        public string NioProductName { get; set; }

        /// <summary>
        /// NIO硬件版本号, max length 64.
        /// </summary>
        public string NioHardwareRevision { get; set; }

        /// <summary>
        /// NIO软件版本号, max length 64.
        /// </summary>
        //public string NioSoftwareRevision { get; set; }

        /// <summary>
        /// NIO 车型, max length 32.
        /// </summary>
        //public string NioModel { get; set; }

        /// <summary>
        /// NIO项目名称, max length 64
        /// </summary>
        public string NioProjectName { get; set; }

        /// <summary>
        /// 是否已投产, true/false
        /// </summary>
        public bool Launched { get; set; }

        /// <summary>
        /// 合作伙伴工厂编码
        /// </summary>
        public string VendorFactoryCode { get; set; }

        /// <summary>
        /// 马威零件版本
        /// </summary>
        public string VendorHardwareRevision { get; set; }
    }
}
