using System.Text.Json.Serialization;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 日志输出
    /// </summary>
    public class HitTraceLogDto 
    {
        /// <summary>
        /// documentId
        /// </summary>
        public string IdentifierId { get; set; }

        /// <summary>
        /// 索引名称
        /// </summary>
        public string IndexName { get; set; }

        /// <summary>
        /// 业务标识
        /// </summary>
        [JsonPropertyName("Id")]
        public string Id { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        [JsonPropertyName("Type")]
        public string Type { get; set; }

        /// <summary>
        /// 日志消息
        /// </summary>
        [JsonPropertyName("Message")]
        public string Message { get; set; }
        /// <summary>
        /// 服务类型
        /// </summary>
        [JsonPropertyName("ServiceType")]
        public string ServiceType { get; set; }
        /// <summary>
        /// 接口编码
        /// </summary>
        [JsonPropertyName("InterfaceCode")]
        public string InterfaceCode { get; set; }

        /// <summary>
        /// 日志数据
        /// </summary>
        [JsonPropertyName("Data")]
        public IReadOnlyDictionary<string, string> Data { get; set; } = new Dictionary<string, string>();


        /// <summary>
        /// 时间戳
        /// </summary>
        [JsonPropertyName("@timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    }
}
