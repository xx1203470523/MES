namespace Hymson.MES.CoreServices.Bos.Parameter
{
    /// <summary>
    /// 出站参数
    /// </summary>
    public record OutStationParameterBo
    {
        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParamCode { get; set; } = string.Empty;

        /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue { get; set; } = string.Empty;

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
