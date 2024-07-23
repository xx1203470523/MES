using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 生产退料单明细新增/更新Dto
    /// </summary>
    public record ManuReturnOrderDetailSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 所属退料单Id
        /// </summary>
        public long ReturnOrderId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string MaterialBarCode { get; set; }

        /// <summary>
        /// 物料批次
        /// </summary>
        public string Batch { get; set; }

        /// <summary>
        /// 退料数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 仓库id
        /// </summary>
        public long? WarehouseId { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// 物料的有效期（过期时间)
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// 物料描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }


    }

    /// <summary>
    /// 生产退料单明细Dto
    /// </summary>
    public record ManuReturnOrderDetailDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 所属退料单Id
        /// </summary>
        public long ReturnOrderId { get; set; }

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
        public string Version { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string MaterialBarCode { get; set; }

        /// <summary>
        /// 物料批次
        /// </summary>
        public string Batch { get; set; }

        /// <summary>
        /// 退料数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 仓库id
        /// </summary>
        public long? WarehouseId { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 物料的有效期（过期时间)
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// 物料描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }


    }

    /// <summary>
    /// 生产退料单明细分页Dto
    /// </summary>
    public class ManuReturnOrderDetailPagedQueryDto : PagerInfo { }

}
