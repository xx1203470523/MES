using Hymson.MES.Core.Enums;
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
        /// 物料数量
        /// </summary>
        /// 
        [JsonPropertyName("quantity")]
        public string Quantity { get; set; }
        /// <summary>
        /// 物料批次号
        /// </summary>
        /// 
        [JsonPropertyName("lotCode")]
        public string LotCode { get; set; }
        /// <summary>
        /// 物料条码
        /// </summary>
        /// 
        [JsonPropertyName("sfc")]
        public string SFC { get; set; }

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

    /// <summary>
    /// 成品入库退料申请单
    /// </summary>
    public record ProductReceiptCancel
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
    /// 入库退料DTO
    /// </summary>
    public record ProductReceiptCancelDto
    {
        /// <summary>
        /// 同步单号
        /// </summary>
        public string SyncCode { get; set; }
        public string SendOn { get; set; }
    }

    /// <summary>
    /// 入库申请单
    /// </summary>
    public record ProductReceiptRequest
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
        public List<ProductReceiptItemDto> Details { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        [JsonPropertyName("createBy")]
        public string CreateBy { get; set; }
    }

    /// <summary>
    /// 成品入库
    /// </summary>
    public record ProductReceiptRequestDto
    {

        public string SyncCode { get; set; }


        public string SendOn { get; set; }

        public string WarehouseCode { get; set; }

        public List<ProductReceiptItemDto> Details { get; set; }

        public string CreateBy { get; set; }
    }
    /// <summary>
    /// 成品入库明细item
    /// </summary>
    public record ProductReceiptItemDto
    {
        /// <summary>
        /// 生产订单号
        /// </summary>
        public string? ProductionOrderNumber { get; set; }

        /// <summary>
        /// 生产订单子表ID
        /// </summary>
        public long? ProductionOrderDetailID { get; set; }

        /// <summary>
        /// 生产订单子件ID
        /// </summary>
        //public long? ProductionOrderComponentID { get; set; }

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
        /// 物料数量
        /// </summary>
        /// 
        [JsonPropertyName("quantity")]
        public string Quantity { get; set; }
        /// <summary>
        /// 物料批次号
        /// </summary>
        /// 
        [JsonPropertyName("lotCode")]
        public string LotCode { get; set; }
        /// <summary>
        /// 物料条码
        /// </summary>
        /// 
        [JsonPropertyName("sfc")]
        public string SFC { get; set; }

        /// <summary>
        /// 箱码
        /// </summary>
        [JsonPropertyName("boxCode")]
        public string? BoxCode { get; set; }

        /// <summary>
        /// 是否为联副产品
        /// </summary>
        [JsonPropertyName("bRelated")]
        public TrueOrFalseEnum? BRelated { get; set; }

        /// <summary>
        /// 唯一码
        /// </summary>
        [JsonPropertyName("uniqueCode")]
        public string UniqueCode {  get; set; }

    }
    /// <summary>
    /// 废成品入库申请单
    /// </summary>
    public record WasteProductReceiptRequest
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
        public List<WasteProductReceiptItemDto> Details { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        [JsonPropertyName("createBy")]
        public string CreateBy { get; set; }
    }
    /// <summary>
    /// 废成品入库
    /// </summary>
    public record WasteProductReceiptRequestDto
    {

        public string SyncCode { get; set; }


        public string SendOn { get; set; }

        public string WarehouseCode { get; set; }

        public List<WasteProductReceiptItemDto> Details { get; set; }

        public string CreateBy { get; set; }
    }
    /// <summary>
    /// 废成品入库明细item
    /// </summary>
    public record WasteProductReceiptItemDto
    {
        /// <summary>
        /// 生产订单号
        /// </summary>
        public string? ProductionOrderNumber { get; set; }

        /// <summary>
        /// 生产订单子表ID
        /// </summary>
        public long? ProductionOrderDetailID { get; set; }

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
        /// 物料数量
        /// </summary>
        /// 
        [JsonPropertyName("quantity")]
        public string Quantity { get; set; }

        /// <summary>
        /// 是否为联副产品
        /// </summary>
        [JsonPropertyName("bRelated")]
        public TrueOrFalseEnum? BRelated {  get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class BaseResponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("msg")]
        public string Message { get; set; } = "";
    }

}
