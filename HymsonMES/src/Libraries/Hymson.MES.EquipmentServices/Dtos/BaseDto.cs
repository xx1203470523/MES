using Hymson.Infrastructure;

namespace Hymson.MES.EquipmentServices
{
    /// <summary>
    /// 
    /// </summary>
    public record BaseDto : BaseEntityDto
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
    public class EquipmentProcessParamInfoDto
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

        /// <summary>
        /// 操作员
        /// </summary>
        public string? CreatedBy { get; set; }
    }

}
