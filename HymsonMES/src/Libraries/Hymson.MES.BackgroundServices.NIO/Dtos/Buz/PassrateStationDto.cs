namespace Hymson.MES.BackgroundServices.NIO.Dtos.Buz
{
    /// <summary>  
    /// 生产数据实体类  
    /// </summary>  
    public class PassrateStationDto
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
        /// 工位唯一标识, 最大长度64  
        /// </summary>  
        public string StationId { get; set; }

        /// <summary>  
        /// 合作伙伴总成产品代码, 最大长度128  
        /// </summary>  
        public string VendorProductNum { get; set; }

        /// <summary>  
        /// 合作伙伴产品名称, 最大长度64  
        /// </summary>  
        public string VendorProductName { get; set; }

        /// <summary>  
        /// 一次合格率，使用小数表示，如 0.9601 表示合格率为96.01%（最大支持10位整数+5位小数）  
        /// </summary>  
        public decimal PassRate { get; set; }

        /// <summary>  
        /// 一次合格率目标值，使用小数表示，如0.96表示合格率目标为96.00%（最大支持10位整数+5位小数）  
        /// </summary>  
        public decimal PassRateTarget { get; set; }

        /// <summary>  
        /// 工单更改的时间, Unix 时间戳, 以秒为单位  
        /// </summary>  
        public long UpdateTime { get; set; }

        /// <summary>  
        /// 调试标志  
        /// </summary>  
        public bool Debug { get; set; }
    }
}
