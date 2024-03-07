using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.WHMaterialReceiptDetail
{
    /// <summary>
    /// 数据实体（物料收货表详细）   
    /// wh_material_receipt_detail
    /// @author Jam
    /// @date 2024-03-04 02:21:07
    /// </summary>
    public class WHMaterialReceiptDetailEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 主表Id
        /// </summary>
        public long MaterialReceiptId { get; set; }

       /// <summary>
        /// 物料Id
        /// </summary>
        public long? MaterialId { get; set; }

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
        /// 计划到货时间
        /// </summary>
        public DateTime? PlanTime { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       
    }
}
