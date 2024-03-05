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
    /// 顷刻能源设备对接参数基础类
    /// </summary>
    public record QknyParamBaseDto
    {
        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParamCode { get; set; } = "";

        /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue { get; set; } = "";

        /// <summary>
        /// 参数采集时间
        /// </summary>
        public DateTime CollectionTime { get; set; }
    }

    /// <summary>
    /// 生产出站基础类
    /// </summary>
    public record QknyOutboundBaseDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; } = "";

        /// <summary>
        /// 0：不合格； 1：合格
        /// </summary>
        public int Passed { get; set; }

        /// <summary>
        /// 参数列表
        /// </summary>
        public List<QknyParamBaseDto> ParamList { get; set; } = new List<QknyParamBaseDto>();

        /// <summary>
        /// 绑定的物料批次条码列表
        /// </summary>
        public List<string> BindFeedingCodeList { get; set; } = new List<string>();

        /// <summary>
        /// 不良原因
        /// </summary>
        public List<string> NgList { get; set; } = new List<string>();
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
