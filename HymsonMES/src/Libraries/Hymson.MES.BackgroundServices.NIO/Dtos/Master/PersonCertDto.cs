namespace Hymson.MES.BackgroundServices.NIO.Dtos.Master
{
    /// <summary>
    /// 人员资质
    /// </summary>
    public class PersonCertDto : BaseDto
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
        /// 工位唯一标识
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 岗位唯一标识
        /// </summary>
        public string PositionId { get; set; }

        /// <summary>
        /// 岗位名称
        /// </summary>
        public string PositionName { get; set; }

        /// <summary>
        /// 操作员账号唯一标识
        /// </summary>
        public string OperatorId { get; set; }

        /// <summary>
        /// 操作员姓名
        /// </summary>
        public string OperatorName { get; set; }

        /// <summary>
        /// 描述上述操作员在对应岗位上是否有资质
        /// </summary>
        public bool Qualified { get; set; }

    }
}
