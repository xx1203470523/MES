namespace Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto
{
    /// <summary>
    /// 
    /// </summary>
    public class SFCOutStationDto
    {
        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode { get; set; }
        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; }
        /// <summary>
        /// 本地时间
        /// </summary>
        public DateTime LocalTime { get; set; }
        /// <summary>
        /// 产品条码
        /// </summary>
        public string SFC { get; set; }
    }
}
