namespace Hymson.MES.EquipmentServicesTests.Dtos
{
    public class ProcessRouteInOutBoundDto
    {
        public string Sort { get; set; }
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
        /// <summary>
        /// 当前工序Id
        /// </summary>
        public long ProcedureId { get; set; }
        /// <summary>
        /// 设备Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 设备编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 工作中心/工厂
        /// </summary>
        public long FactoryId { get; set; }
        /// <summary>
        /// 设备编码
        /// </summary>
        public string ResourceCode { get; set; }
    }
}
