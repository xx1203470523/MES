using Hymson.Infrastructure;

namespace Hymson.MES.SystemServices.Dtos
{
    /// <summary>
    /// IQC请求Dto
    /// </summary>
    public record WhMaterialReceiptDto : BaseEntityDto
    {
        /// <summary>
        /// 收货单号
        /// </summary>
        public string ReceiptNum { get; set; } = "";

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode { get; set; } = "";

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 同步编码（WMS需要给ERP）
        /// </summary>
        public string? SyncCode { get; set; } = "";

        /// <summary>
        /// 同步Id（WMS需要给ERP）
        /// </summary>
        public long? SyncId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<WhMaterialReceiptMaterialDto>? Details { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public record WhMaterialReceiptMaterialDto : BaseEntityDto
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; } = "";

        /// <summary>
        /// 供应商生产批次
        /// </summary>
        public string SupplierBatch { get; set; } = "";

        /// <summary>
        /// 内部批次
        /// </summary>
        public string InternalBatch { get; set; } = "";

        /// <summary>
        /// 计划发货数量
        /// </summary>
        public decimal? PlanQty { get; set; }

        /// <summary>
        /// 实收数量
        /// </summary>
        public decimal? Qty { get; set; }

        /// <summary>
        /// 实际到货时间
        /// </summary>
        public DateTime? ActualTime { get; set; }

        /// <summary>
        /// 计划到货时间
        /// </summary>
        public DateTime? PlanTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 条码（唯一码）
        /// </summary>
        public string? BarCode { get; set; } = "";

        /// <summary>
        /// 同步Id（WMS需要给ERP）
        /// </summary>
        public long? SyncId { get; set; }

    }

}
