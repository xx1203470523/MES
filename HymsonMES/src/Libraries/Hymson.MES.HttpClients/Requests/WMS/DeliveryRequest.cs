namespace Hymson.MES.HttpClients.Requests.XnebulaWMS
{
    /// <summary>
    /// 
    /// </summary>
    public  class DeliveryDetailDto
    {
        /*
        /// <summary>
        /// 同步明细ID
        /// </summary>
        public long? SyncId { get; set; }
        */

        /// <summary>
        /// 生产订单号
        /// </summary>
        public string? ProductionOrder { get; set; }

        /// <summary>
        /// 子工单号
        /// </summary>
        public string? WorkOrderCode { get; set; }

        /// <summary>
        /// 生产订单子表ID
        /// </summary>
        public long? ProductionOrderDetailID { get; set; }

        /// <summary>
        /// 生产订单子件ID
        /// </summary>
        public long? ProductionOrderComponentID { get; set; }

        /// <summary>
        /// 物料编号
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料分类编号
        /// </summary>
        public string? MaterialClassificationCode { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string? LotCode { get; set; }

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
    }

    /// <summary>
    /// 出库单
    /// </summary>
    public record DeliveryRequest 
    {
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
        /// 单据类型
        /// 301 采购退货单
        /// 302 销售订单
        /// 303 其他出库单
        /// 304 材料申请单
        /// </summary>
        public BillBusinessTypeEnum Type { get; set; }

        /// <summary>
        /// 客户编号
        /// </summary>
        public string? CustomerCode { get; set; }

        /// <summary>
        /// 供应商编号
        /// </summary>
        public string? SupplierCode { get; set; }

        /// <summary>
        /// 出库类别编码
        /// </summary>
        public string? StockOutCategory { get; set; }

        /// <summary>
        /// 是否自动执行
        /// </summary>
        public bool? IsAutoExecute { get; set; }

        /// <summary>
        /// 上游制单人
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 明细集
        /// </summary>
        public IEnumerable<DeliveryDetailDto>? Details { get; set; }
    }

    /// <summary>
    /// 出库单
    /// </summary>
    public record DeliveryDto
    {
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
        /// 单据类型
        /// 301 采购退货单
        /// 302 销售订单
        /// 303 其他出库单
        /// 304 材料申请单
        /// </summary>
        public BillBusinessTypeEnum Type { get; set; }

        /// <summary>
        /// 客户编号
        /// </summary>
        public string? CustomerCode { get; set; }

        /// <summary>
        /// 供应商编号
        /// </summary>
        public string? SupplierCode { get; set; }

        /// <summary>
        /// 出库类别编码
        /// </summary>
        public string? StockOutCategory { get; set; }

        /// <summary>
        /// 是否自动执行
        /// </summary>
        public bool? IsAutoExecute { get; set; }

        /// <summary>
        /// 上游制单人
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 明细集
        /// </summary>
        public IEnumerable<DeliveryDetailDto>? Details { get; set; }
    }
}
