namespace Hymson.MES.EquipmentServices.Bos
{
    /// 请求参数（设备产品参数）
    /// </summary>
    public class EquipmentProductParamBo
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; } = "";

        /// <summary>
        ///  参数Id
        /// </summary>
        public long ParameterId { get; set; }

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
