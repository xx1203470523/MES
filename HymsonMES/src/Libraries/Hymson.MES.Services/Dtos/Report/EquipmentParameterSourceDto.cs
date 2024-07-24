using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Report
{
    /// <summary>
    /// 设备参数追溯DTO
    /// </summary>
    public record EquipmentParameterSourceDto:BaseEntityDto
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName { get; set; }
        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParameterCode { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public string ParameterValue { get; set; } = "";

        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime CollectionTime { get; set; }
    }
}
