using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.WHMaterialReceiptDetail
{
    /// <summary>
    /// 数据实体（收料单详情）   
    /// wh_material_receipt_detail
    /// @author Jam
    /// @date 2024-03-08 02:27:34
    /// </summary>
    public class WHMaterialReceiptDetailEntity : BaseEntity
    {
        /// <summary>
        /// wh_material_receipt的id
        /// </summary>
        public long MaterialReceiptId { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 物料批次码
        /// </summary>
        public string MaterialBatchCode { get; set; }

        /// <summary>
        /// 供应商生产批次
        /// </summary>
        public string SupplierBatch { get; set; }

        /// <summary>
        /// 内部批次
        /// </summary>
        public string InternalBatch { get; set; }

        /// <summary>
        /// 计划发货数量
        /// </summary>
        public decimal? PlanQty { get; set; }

        /// <summary>
        /// 实收数量
        /// </summary>
        public decimal? Qty { get; set; }

        /// <summary>
        /// 计划到货时间
        /// </summary>
        public DateTime? PlanTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 条码（唯一码）
        /// </summary>
        public string? BarCode { get; set; }

        /// <summary>
        /// 同步Id（WMS需要给ERP）
        /// </summary>
        public long? SyncId { get; set; }

    }
}
