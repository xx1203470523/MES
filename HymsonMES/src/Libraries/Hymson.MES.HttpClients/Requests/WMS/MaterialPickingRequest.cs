﻿using System.Text.Json.Serialization;

namespace Hymson.MES.HttpClients.Requests
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

    /// <summary>
    /// 
    /// </summary>
    public record MaterialPickingRequestDto
    {
        /// <summary>
        /// 同步单号
        /// </summary>
        public string SyncCode { get; set; }
        /// <summary>
        /// 下发日期
        /// </summary>
        public string SendOn { get; set; }
        /// <summary>
        /// 领料信息
        /// </summary>
        public List<ProductionPickMaterialDto> details { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    internal record MaterialPickingCancel
    {
        /// <summary>
        /// 同步单号
        /// </summary>
        [JsonPropertyName("syncCode")]
        public string SyncCode { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("UpdatedBy")]
        public string UpdatedBy { get; set; } = "";

    }

    /// <summary>
    /// 
    /// </summary>
    public record MaterialPickingCancelDto
    {
        /// <summary>
        /// 同步单号
        /// </summary>
        public string SyncCode { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        public string UpdatedBy { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        public string SendOn { get; set; } = "";

    }

    /// <summary>
    /// 
    /// </summary>
    public class ProductionPickMaterialDto
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        [JsonPropertyName("materialCode")]
        public string MaterialCode { get; set; }

        /// <summary>
        /// 单位编号
        /// </summary>
        [JsonPropertyName("unitCode")]
        public string UnitCode { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        [JsonPropertyName("quantity")]
        public string Quantity { get; set; }

        /// <summary>
        /// 生产订单号
        /// </summary>
        [JsonPropertyName("productionOrder")]
        public string? ProductionOrder { get; set; }

        /// <summary>
        /// 子工单号
        /// </summary>
        [JsonPropertyName("workOrderCode")]
        public string? WorkOrderCode { get; set; }

        /// <summary>
        /// 生产订单子表ID
        /// </summary>
        [JsonPropertyName("productionOrderDetailID")]
        public long? ProductionOrderDetailID { get; set; }

        /// <summary>
        /// 生产订单子件ID
        /// </summary>
        [JsonPropertyName("productionOrderComponentID")]
        public long? ProductionOrderComponentID { get; set; }

    }

}
