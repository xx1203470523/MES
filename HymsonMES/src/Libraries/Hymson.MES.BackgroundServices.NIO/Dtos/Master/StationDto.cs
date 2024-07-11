namespace Hymson.MES.BackgroundServices.NIO.Dtos.Master
{
    /// <summary>
    /// 产品主数据
    /// </summary>
    public class StationDto : BaseDto
    {
        /// <summary>
        /// 工厂唯一标识
        /// </summary>
        public string PlantId { get; set; }

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string PlantName { get; set; }

        /// <summary>
        /// 车间唯一标识
        /// </summary>
        public string WorkshopId { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkshopName { get; set; }

        /// <summary>
        /// 生产线唯一标识
        /// </summary>
        public string ProductionLineId { get; set; }

        /// <summary>
        /// 生产线名称
        /// </summary>
        public string ProductionLineName { get; set; }

        /// <summary>
        /// 工位唯一标识
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 工位名称
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ProductionLineOrder { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int StationOrder { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool KeyStation { get; set; }

        /// <summary>
        /// 合作伙伴总成产品代码
        /// </summary>
        public string VendorProductNum { get; set; }

    }
}
