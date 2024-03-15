using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.WHMaterialReceipt
{
    /// <summary>
    /// 物料收货表新增/更新Dto
    /// </summary>
    public record WhMaterialReceiptSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 收货单号
        /// </summary>
        public string ReceiptNum { get; set; }

        /// <summary>
        /// 供应商Id
        /// </summary>
        public long SupplierId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public IList<WhMaterialReceiptDetailSaveDto> Details { get; set; }


    }

    public record WhMaterialReceiptDetailSaveDto : BaseEntityDto
    {
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


    }

    /// <summary>
    /// 物料收货表Dto
    /// </summary>
    public record WhMaterialReceiptDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 收货单号
        /// </summary>
        public string ReceiptNum { get; set; }

        /// <summary>
        /// 供应商Id
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
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summar
        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

    }


    public record WhMaterialReceiptOutDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 收货单号
        /// </summary>
        public string ReceiptNum { get; set; }

        /// <summary>
        /// 供应商Id
        /// </summary>
        public long SupplierId { get; set; }

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
    /// 物料收货表分页Dto
    /// </summary>
    public class WhMaterialReceiptPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 收货单号
        /// </summary>
        public string? ReceiptNum { get; set; }

        /// <summary>
        /// 编码（物料）
        /// </summary>
        public string? SupplierCode { get; set; }

        /// <summary>
        /// 名称（物料）
        /// </summary>
        public string? SupplierName { get; set; }

    }

}
