using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hymson.MES.HttpClients.Requests.Print
{
    /// <summary>
    /// 生产领料申请单
    /// </summary>
    internal record MaterialPickingRequest
    {
        /// <summary>
        /// 仓库编号
        /// </summary>
        [JsonPropertyName("warehouseCode")]
        public string WarehouseCode { get; set; }
        /// <summary>
        /// 同步单号
        /// </summary>
        [JsonPropertyName("syncCode")]
        public string SyncCode { get; set; }
        [JsonPropertyName("type")]
        public int Type { get; set; } = 304;
        /// <summary>
        /// 下发日期
        /// </summary>
        [JsonPropertyName("sendOn")]
        public string SendOn { get; set; }
        /// <summary>
        /// 领料信息
        /// </summary>
        [JsonPropertyName("details")]
        public List<ProductionPickMaterialDto> Details { get; set; }
    }
    public record MaterialPickingRequestDto
    {
        
        /// <summary>
        /// 同步单号
        /// </summary>
        public string syncCode { get; set; }
        /// <summary>
        /// 下发日期
        /// </summary>
        public string sendOn { get; set; }
        /// <summary>
        /// 领料信息
        /// </summary>
        public List<ProductionPickMaterialDto> details { get; set; }
    }
    public record MaterialPickingCancelDto
    {
        /// <summary>
        /// 同步单号
        /// </summary>
        public string SyncCode { get; set; }
       
    }

    public class ProductionPickMaterialDto
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string unitCode { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string quantity { get; set; }

    }
   
}
