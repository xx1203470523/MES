namespace Hymson.MES.EquipmentServices
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseRequest
    {
        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode { get; set; } = "";

        /// <summary>
        /// 设备编码(只接收不使用使用令牌解析出设备编码)
        /// </summary>
        public string EquipmentCode { get; set; } = "";

        /// <summary>
        /// 设备调用本地时间
        /// </summary>
        public DateTime LocalTime { get; set; }
    }

    /// 请求参数（设备参数信息）
    /// </summary>
    public class EquipmentProcessParamInfoRequest
    {
        /// <summary>
        ///  参数编码
        /// </summary>
        public string ParamCode { get; set; } = "";

        /// <summary>
        ///  参数值
        /// </summary>
        public string ParamValue { get; set; } = "";

        /// <summary>
        ///  时间戳（参数采集到的时间）
        /// </summary>
        public DateTime Timestamp { get; set; }
    }

}
