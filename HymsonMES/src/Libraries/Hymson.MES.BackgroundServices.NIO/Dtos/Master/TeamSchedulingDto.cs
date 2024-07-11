namespace Hymson.MES.BackgroundServices.NIO.Dtos.Master
{
    /// <summary>
    /// 排班
    /// </summary>
    public class TeamSchedulingDto : BaseDto
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
        /// 班次唯一标识
        /// </summary>
        public string TeamId { get; set; }

        /// <summary>
        /// 班次名称
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// 班次开始时间
        /// </summary>
        public string TeamStartTime { get; set; }

        /// <summary>
        /// 班次结束时间
        /// </summary>
        public string TeamEndTime { get; set; }

    }
}
