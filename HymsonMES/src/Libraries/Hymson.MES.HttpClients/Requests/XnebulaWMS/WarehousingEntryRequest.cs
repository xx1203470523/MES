using Hymson.Infrastructure;

namespace Hymson.MES.HttpClients.Requests.XnebulaWMS
{
    public class WarehousingEntryRequest
    {
        /// <summary>
        /// 单据类型
        /// 101 到货单
        /// 102 销售退货单
        /// 103 其他入库单（保存）
        /// </summary>
        public BillBusinessTypeEnum Type { get; set; }

        /// <summary>
        /// 仓库编号
        /// </summary>
        public string? WarehouseCode { get; set; }

        /// <summary>
        /// 同步单号
        /// </summary>
        public string? SyncCode { get; set; }

        /// <summary>
        /// 下发日期
        /// </summary>
        public DateTime SendOn { get; set; }

        /// <summary>
        /// 供应商编号
        /// </summary>
        public string? SupplierCode { get; set; }

        /// <summary>
        /// 客户编码
        /// </summary>
        public string? CustomerCode { get; set; }

        /// <summary>
        /// 采购类型
        /// </summary>
        public string? PurchaseType { get; set; }

        /// <summary>
        /// 入库类别编码
        /// </summary>
        public string? InboundCategory { get; set; }

        /// <summary>
        /// 是否自动执行
        /// </summary>
        public bool? IsAutoExecute { get; set; }

        /// <summary>
        /// 制单人
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 明细集
        /// </summary>
        public IEnumerable<ReceiptDetailDto>? Details { get; set; }
    }

    /// <summary>
    /// 入库单明细
    /// </summary>
    public record ReceiptDetailDto : BaseEntityDto
    {
        /// <summary>
        /// 生产订单号
        /// </summary>
        public string? ProductionOrder { get; set; }

        /// <summary>
        /// 生产订单子表ID
        /// </summary>
        public long? ProductionOrderDetailID { get; set; }

        /// <summary>
        /// 生产订单子件ID
        /// </summary>
        public long? ProductionOrderComponentID { get; set; }

        /// <summary>
        /// 生产订单号（大工单）
        /// </summary>
        public string? ProductionOrderNumber { get; set; }

        /// <summary>
        /// 子工单号
        /// </summary>
        public string? WorkOrderCode { get; set; }

        /// <summary>
        /// 同步明细ID
        /// </summary>
        public long? SyncId { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string? MaterialBarCode { get; set; }

        /// <summary>
        /// 物料编号
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string? LotCode { get; set; }

        /// <summary>
        /// 箱码
        /// </summary>
        public string? BoxCode { get; set; }

        /// <summary>
        /// 物料分类编号
        /// </summary>
        public string? MaterialClassificationCode { get; set; }

        /// <summary>
        /// 唯一码
        /// </summary>
        public string? UniqueCode { get; set; }

        /// <summary>
        /// 单位编号
        /// </summary>
        public string? UnitCode { get; set; }

        /// <summary>
        /// 单位组编号
        /// </summary>
        public string? UnitGroupCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 批次号（领料时，WMS传过来的值，要求MES必须返回）
        /// </summary>
        public string Batch { get; set; } = "";

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

    }

    /// <summary>
    /// 
    /// </summary>
    public record WarehousingEntryDto
    {
        /// <summary>
        /// 单据类型
        /// </summary>
        public BillBusinessTypeEnum Type { get; set; }

        /// <summary>
        /// 仓库编号
        /// </summary>
        public string? WarehouseCode { get; set; }

        /// <summary>
        /// 同步单号
        /// </summary>
        public string? SyncCode { get; set; }

        /// <summary>
        /// 下发日期
        /// </summary>
        public DateTime SendOn { get; set; }

        /// <summary>
        /// 供应商编号
        /// </summary>
        public string? SupplierCode { get; set; }

        /// <summary>
        /// 客户编码
        /// </summary>
        public string? CustomerCode { get; set; }

        /// <summary>
        /// 采购类型
        /// </summary>
        public string? PurchaseType { get; set; }

        /// <summary>
        /// 入库类别编码
        /// </summary>
        public string? InboundCategory { get; set; }

        /// <summary>
        /// 是否自动执行
        /// </summary>
        public bool? IsAutoExecute { get; set; }

        /// <summary>
        /// 制单人
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 明细集
        /// </summary>
        public IEnumerable<ReceiptDetailDto>? Details { get; set; }

    }
}
