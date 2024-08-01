namespace Hymson.MES.BackgroundServices.NIO.Dtos.Buz
{
    /// <summary>  
    /// 工单与生产数据实体类  
    /// </summary>  
    public class WorkOrderDto : BaseDto
    {
        /// <summary>  
        /// 工厂唯一标识, 最大长度64  
        /// </summary>  
        public string PlantId { get; set; }

        /// <summary>  
        /// 车间唯一标识, 最大长度64  
        /// </summary>  
        public string WorkshopId { get; set; }

        /// <summary>  
        /// 生产线唯一标识, 最大长度64  
        /// </summary>  
        public string ProductionLineId { get; set; }

        /// <summary>  
        /// 工单号，工单唯一标识, 最大长度64  
        /// </summary>  
        public string WorkorderId { get; set; }

        /// <summary>  
        /// 工单创建的时间，unix时间戳，以秒为单位  
        /// </summary>  
        public long OrderCreateTime { get; set; }

        /// <summary>  
        /// 合作伙伴产品代码, 最大长度64  
        /// </summary>  
        public string VendorProductCode { get; set; }

        ///// <summary>  
        ///// 合作伙伴产品名称, 最大长度64  
        ///// </summary>  
        //public string VendorProductName { get; set; }

        ///// <summary>  
        ///// NIO 产品电子条码, 最大长度64  
        ///// </summary>  
        //public string NioProductCode { get; set; }

        ///// <summary>  
        ///// NIO 产品代码, 最大长度128  
        ///// 成品, 半成品, 原材料统一称呼. 同一个型号的产品拥有相同的产品代码.  
        ///// </summary>  
        //public string NioProductNum { get; set; }

        ///// <summary>  
        ///// NIO 产品名称, 最大长度64  
        ///// 示例: ES8尾门总成  
        ///// </summary>  
        //public string NioProductName { get; set; }

        ///// <summary>  
        ///// NIO 车型, 最大长度32  
        ///// 示例: ES8, ET7  
        ///// </summary>  
        //public string NioModel { get; set; }

        /// <summary>  
        /// 数量，整数  
        /// </summary>  
        public int Quantity { get; set; }

        ///// <summary>  
        ///// NIO硬件版本号, 最大长度64  
        ///// </summary>  
        //public string NioHardwareRevision { get; set; }

        ///// <summary>  
        ///// NIO软件版本号, 最大长度64  
        ///// </summary>  
        //public string NioSoftwareRevision { get; set; }

        ///// <summary>  
        ///// NIO项目名称, 最大长度64（可选）  
        ///// </summary>  
        //public string NioProjectName { get; set; }
    }
}
