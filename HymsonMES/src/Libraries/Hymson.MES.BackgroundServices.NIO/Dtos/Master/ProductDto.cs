using Newtonsoft.Json;

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
        [JsonProperty("vendorProductCode")]
        public string VendorProductCode { get; set; }

        /// <summary>
        /// 合作伙伴产品名称，max length 64
        /// </summary>
        [JsonProperty("vendorProductName")]
        public string VendorProductName { get; set; }

        /// <summary>
        /// NIO产品料号, max length 64.
        /// </summary>
        [JsonProperty("nioProductCode")]
        public string NioProductCode { get; set; }

        /// <summary>
        /// NIO 产品名称, max length 64
        /// </summary>
        [JsonProperty("nioProductName")]
        public string NioProductName { get; set; }

        /// <summary>
        /// NIO硬件版本号, max length 64.
        /// </summary>
        [JsonProperty("nioHardwareRevision")]
        public string NioHardwareRevision { get; set; }

        /// <summary>
        /// NIO软件版本号, max length 64.
        /// </summary>
        [JsonProperty("nioSoftwareRevision")]
        public string NioSoftwareRevision { get; set; }

        /// <summary>
        /// NIO 车型, max length 32.
        /// </summary>
        [JsonProperty("nioModel")]
        public string NioModel { get; set; }

        /// <summary>
        /// NIO项目名称, max length 64
        /// </summary>
        [JsonProperty("nioProjectName")]
        public string NioProjectName { get; set; }

        /// <summary>
        /// 是否已投产, true/false
        /// </summary>
        [JsonProperty("launched")]
        public bool Launched { get; set; }

    }
}
