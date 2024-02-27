using Hymson.Infrastructure;

namespace Hymson.MES.EquipmentServices
{
    /// <summary>
    /// 顷刻能源设备对接基础类
    /// </summary>
    public record QknyBaseDto : BaseEntityDto
    {
        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; } = "";

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode { get; set; } = "";

        /// <summary>
        /// 设备调用本地时间
        /// </summary>
        public DateTime LocalTime { get; set; }
    }

    /// <summary>
    /// 顷刻能源设备对接返回结果基础类
    /// </summary>
    public record QknyReturnBaseDto
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Msg { get; set; } = "";
    }

}
