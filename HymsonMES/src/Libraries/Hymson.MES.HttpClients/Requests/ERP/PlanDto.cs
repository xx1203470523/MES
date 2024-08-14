using Hymson.Infrastructure;
using System.Text.Json.Serialization;

namespace Hymson.MES.HttpClients.Requests.ERP
{
    /// <summary>
    /// 请求Dto
    /// </summary>
    public record PlanRequestDto : BaseEntityDto
    {
        /// <summary>
        /// 类型（0:整单;1:明细;）
        /// </summary>
        public int type { get; set; } = 1;

        /// <summary>
        /// 生产订单Id（type=0时，必填）
        /// </summary>
        public long? moid { get; set; }

        /// <summary>
        /// 生产订单状态（0:关闭;1:打开）（type=0时，必填）
        /// </summary>
        public int mostate { get; set; }

        /// <summary>
        /// 同步编码（WMS需要给ERP）
        /// </summary>
        public IEnumerable<PlanProductRequestDto>? Deteils { get; set; }    // 没有写成 details，是因为ERP就是写的 deteils

    }

    /// <summary>
    /// 请求Dto
    /// </summary>
    public record PlanProductRequestDto : BaseEntityDto
    {
        /// <summary>
        /// 生产订单明细Id
        /// </summary>
        public long modid { get; set; }

        /// <summary>
        /// 生产订单明细状态（0:关闭;1:打开）
        /// </summary>
        public int modstate { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class BaseERPResponse
    {
        [JsonPropertyName("status")]
        public bool Status { get; set; }

        [JsonPropertyName("msg")]
        public string Message { get; set; } = "";
    }

    /// <summary>
    /// 物料相关信息
    /// </summary>
    public record MaterialRequest
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public List<string> MaterialCodeList { get; set; } = new List<string>();

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }
    }


}
