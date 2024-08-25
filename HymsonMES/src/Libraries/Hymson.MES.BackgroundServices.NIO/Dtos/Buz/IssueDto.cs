namespace Hymson.MES.BackgroundServices.NIO.Dtos.Buz
{
    /// <summary>
    /// 缺陷数据
    /// </summary>
    public class IssueDto : BaseDto
    {
        /// <summary>  
        /// 工厂唯一标识，最大长度64  
        /// </summary>  
        public string PlantId { get; set; }

        /// <summary>  
        /// 车间唯一标识，最大长度64  
        /// </summary>  
        public string WorkshopId { get; set; }

        /// <summary>  
        /// 生产线唯一标识，最大长度64  
        /// </summary>  
        public string ProductionLineId { get; set; }

        /// <summary>  
        /// 工位唯一标识，最大长度64  
        /// </summary>  
        public string StationId { get; set; }

        /// <summary>  
        /// 合作伙伴总成产品代码，最大长度128  
        /// </summary>  
        public string VendorProductNum { get; set; }

        ///// <summary>  
        ///// 合作伙伴产品名称，最大长度64  
        ///// </summary>  
        //public string VendorProductName { get; set; }

        ///// <summary>  
        ///// 合作伙伴总成电子条码，最大长度64  
        ///// </summary>  
        //public string VendorProductCode { get; set; }

        /// <summary>  
        /// 合作伙伴总成序列号，最大长度64  
        /// </summary>  
        public string VendorProductSn { get; set; }

        /// <summary>  
        /// 合作伙伴总成临时序列号，最大长度64，可选  
        /// </summary>  
        public string VendorProductTempSn { get; set; }

        /// <summary>  
        /// 合作伙伴定义的缺陷代码，最大长度64  
        /// </summary>  
        public string VendorIssueCode { get; set; }

        /// <summary>  
        /// 合作伙伴定义的缺陷名字，最大长度50  
        /// </summary>  
        public string VendorIssueName { get; set; }
    }

    /// <summary>
    /// NIO推送数据
    /// </summary>
    public class NioIssueDto
    {
        /// <summary>
        /// 标识码
        /// </summary>
        public string SchemaCode { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public List<IssueDto> List = new List<IssueDto>();
    }
}
