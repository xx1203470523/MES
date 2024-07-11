using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hymson.MES.HttpClients.Requests
{
   
    /// <summary>
    /// 转子产线工单同步
    /// </summary>
    public record RotorWorkOrderSync
    {
        /// <summary>
        /// 工单/排程号
        /// </summary>
        /// 
        [JsonPropertyName("workNo")]
        public string OrderCode { get; set; }

        /// <summary>
        /// 生产计划编码
        /// </summary>
        /// 
        [JsonPropertyName("orderNo")]
        public string PlanWorkOrder { get; set; }
        /// <summary>
        /// 型号号+_+版本号
        /// </summary>
        /// 
        [JsonPropertyName("productTypeNO")]
        public string productTypeNO { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        /// 
        [JsonPropertyName("itemNo")]
        public string ProductCode { get; set; }

        /// <summary>
        /// 工作中心（车间或者线体）
        /// </summary>
        /// 
        [JsonPropertyName("workTotal")]
        public long? WorkCenterId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        ///
        [JsonPropertyName("workTotal")]
        public decimal Qty { get; set; }
        /// <summary>
        /// 生产计划数量
        /// </summary>
        ///
        [JsonPropertyName("orderTotal")]
        public decimal PlanQty { get; set; }


        ///// <summary>
        ///// 计划开始时间
        ///// </summary>
        ///// 
        //[JsonPropertyName("planTime")]
        //public DateTime? PlanStartTime { get; set; }

        ///// <summary>
        ///// 计划结束时间
        ///// </summary>
        ///// 
        //[JsonPropertyName("endTime")]
        //public DateTime? PlanEndTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("enable")]
        public bool Enable { get; set; } = true;
        /// <summary>
        /// 版本号
        /// </summary>
        [JsonPropertyName("versionNo")]
        public string VersionNo { get; set; } = "1";

    }
}
