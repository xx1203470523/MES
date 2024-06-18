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
        /// 标准上限
        /// </summary>
        public string? StandardUpperLimit { get; set; }

        /// <summary>
        /// 标准下限
        /// </summary>
        public string? StandardLowerLimit { get; set; }

        /// <summary>
        /// 判定结果
        /// </summary>
        public string? JudgmentResult { get; set; }

        /// <summary>
        /// 测试时长
        /// </summary>
        public string? TestDuration { get; set; }

        /// <summary>
        /// 测试时间
        /// </summary>
        public string? TestTime { get; set; }

        /// <summary>
        /// 测试结果
        /// </summary>
        public string? TestResult { get; set; }

        /// <summary>
        ///  时间戳（参数采集到的时间）
        /// </summary>
        public DateTime Timestamp { get; set; }

    }
}
