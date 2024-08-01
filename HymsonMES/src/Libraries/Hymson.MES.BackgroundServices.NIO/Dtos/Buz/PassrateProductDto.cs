namespace Hymson.MES.BackgroundServices.NIO.Dtos.Buz
{
    /// <summary>  
    /// 产品一次合格率  
    /// </summary>  
    public class PassrateProductDto : BaseDto
    {
        /// <summary>  
        /// 工厂唯一标识  
        /// </summary>  
        public string PlantId { get; set; }

        /// <summary>  
        /// 车间唯一标识（如果适用）  
        /// </summary>  
        public string WorkshopId { get; set; }

        /// <summary>  
        /// 生产线唯一标识（如果适用）  
        /// </summary>  
        public string ProductionLineId { get; set; }

        /// <summary>  
        /// 合作伙伴总成产品代码  
        /// </summary>  
        public string VendorProductNum { get; set; }

        /// <summary>  
        /// 合作伙伴总成物料名称  
        /// </summary>  
        public string VendorProductName { get; set; }

        /// <summary>  
        /// 合格率（百分比形式，例如：99）  
        /// </summary>  
        public decimal PassRate { get; set; }

        /// <summary>  
        /// 合格率目标（百分比形式，例如：98）  
        /// </summary>  
        public decimal PassRateTarget { get; set; }
    }
}
