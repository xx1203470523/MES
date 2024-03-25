using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.WHMaterialReceiptDetail
{
    /// <summary>
    /// 收料单详情新增/更新Dto
    /// </summary>
    public record WHMaterialReceiptDetailSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

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

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       
    }

    /// <summary>
    /// 收料单详情Dto
    /// </summary>
    public record WHMaterialReceiptDetailDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

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
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       
    }


    /// <summary>
    /// 收货物料
    /// </summary>
    public record ReceiptMaterialDetailDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 收货单号
        /// </summary>
        public string ReceiptNum { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public long SupplierId { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 供应商批次
        /// </summary>
        public string SupplierBatch { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string MaterialVersion { get; set; }

        /// <summary>
        /// 内部
        /// </summary>
        public string InternalBatch { get; set; }

        /// <summary>
        /// 计划发货数量
        /// </summary>
        public decimal PlanQty { get; set; }

        /// <summary>
        /// 计划到货时间
        /// </summary>
        public DateTime? PlanTime { get; set; }

    }

    /// <summary>
    /// 收料单详情分页Dto
    /// </summary>
    public class WHMaterialReceiptDetailPagedQueryDto : PagerInfo { }

}
