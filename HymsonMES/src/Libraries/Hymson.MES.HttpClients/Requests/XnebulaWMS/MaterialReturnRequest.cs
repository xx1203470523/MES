using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hymson.MES.HttpClients.Requests
{
    /// <summary>
    /// 生产退料申请单
    /// </summary>
    public record MaterialReturnRequest
    {
        /// <summary>
        /// 仓库编号
        /// </summary>
        [JsonPropertyName("warehouseCode")]
        public string WarehouseCode { get; set; }
        /// <summary>
        /// 同步单号
        /// </summary>
        /// 
        [JsonPropertyName("syncCode")]
        public string SyncCode { get; set; }
        [JsonPropertyName("type")]
        public int Type { get; set; } = 101;
        /// <summary>
        /// 下发日期
        /// </summary>
        /// 
        [JsonPropertyName("sendOn")]
        public string SendOn { get; set; }
        /// <summary>
        /// 退料信息
        /// </summary>
        /// 
        [JsonPropertyName("details")]
        public List<ProductionReturnMaterialItemDto> Details { get; set; }
    }
    public record MaterialReturnRequestDto
    {
 
        public string SyncCode { get; set; }
        
       
        public string SendOn { get; set; }
        
        public List<ProductionReturnMaterialItemDto> Details { get; set; }
    }
    /// <summary>
    /// 退料item
    /// </summary>
    public record ProductionReturnMaterialItemDto
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        /// 
        [JsonPropertyName("materialCode")]
        public string MaterialCode { get; set; }

        /// <summary>
        /// 单位编号
        /// </summary>
        /// 
        [JsonPropertyName("unitCode")]
        public string UnitCode { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        /// 
        [JsonPropertyName("quantity")]
        public string Quantity { get; set; }

    }

    /// <summary>
    /// 生产退料申请单
    /// </summary>
    public record MaterialReturnCancel
    {
        /// <summary>
        /// 仓库编号
        /// </summary>
        [JsonPropertyName("warehouseCode")]
        public string WarehouseCode { get; set; }
        /// <summary>
        /// 同步单号
        /// </summary>
        /// 
        [JsonPropertyName("syncCode")]
        public string SyncCode { get; set; }
        [JsonPropertyName("type")]
        public int Type { get; set; } = 101;
        /// <summary>
        /// 下发日期
        /// </summary>
        /// 
        [JsonPropertyName("sendOn")]
        public string SendOn { get; set; }

    }
    /// <summary>
    /// 生产退料DTO
    /// </summary>
    public record MaterialReturnCancelDto
    {
        /// <summary>
        /// 同步单号
        /// </summary>
        public string SyncCode { get; set; }
        public string SendOn { get; set; }
    }

}
