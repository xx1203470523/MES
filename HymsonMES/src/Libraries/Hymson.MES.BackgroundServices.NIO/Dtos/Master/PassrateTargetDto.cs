namespace Hymson.MES.BackgroundServices.NIO.Dtos.Master
{
    /// <summary>
    /// 一次合格率目标
    /// </summary>
    public class PassrateTargetDto : BaseDto
    {
        /// <summary>
        /// 工厂唯一标识
        /// </summary>
        public string PlantId { get; set; }

        /// <summary>
        /// 车间唯一标识
        /// </summary>
        public string WorkshopId { get; set; }

        /// <summary>
        /// 生产线唯一标识
        /// </summary>
        public string ProductionLineId { get; set; }

        /// <summary>
        /// 生产线唯一标识
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 合合作伙伴产品代码
        /// </summary>
        public string VendorProductCode { get; set; }

        /// <summary>
        /// 合作伙伴产品名称
        /// </summary>
        public string VendorProductName { get; set; }

        /// <summary>
        /// 合格率类别, 用以区分产品维度(product)和工位(station)维度的合格率，值：product或者station
        /// </summary>
        public string PassRateType { get; set; }

        /// <summary>
        /// 一次合格率目标值，使用小数表示，如0.96表示合格率目标为96.00%（最大支持10位整数+5位小数）
        /// </summary>
        public string PassRateTarget { get; set; }

    }

    /// <summary>
    /// NIO推送数据
    /// </summary>
    public class NioPassrateTargetDto
    {
        /// <summary>
        /// 标识码
        /// </summary>
        public string SchemaCode { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public List<PassrateTargetDto> List { get; set; } = new List<PassrateTargetDto>();
    }
}
