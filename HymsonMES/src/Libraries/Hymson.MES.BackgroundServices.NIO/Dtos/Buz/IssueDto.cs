namespace Hymson.MES.BackgroundServices.NIO.Dtos.Buz
{
    public class IssueDto
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

        /// <summary>  
        /// 合作伙伴产品名称，最大长度64  
        /// </summary>  
        public string VendorProductName { get; set; }

        /// <summary>  
        /// 合作伙伴总成电子条码，最大长度64  
        /// </summary>  
        public string VendorProductCode { get; set; }

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

        /// <summary>  
        /// 工单更改的时间，Unix时间戳，以秒为单位  
        /// </summary>  
        public long UpdateTime { get; set; }

        /// <summary>  
        /// 调试标志  
        /// </summary>  
        public bool Debug { get; set; }
    }
}
