using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// OQC检验单新增/更新Dto
    /// </summary>
    public record QualOqcOrderSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 检验单号
        /// </summary>
        public string InspectionOrder { get; set; }

        /// <summary>
        /// OQC检验参数组快照Id
        /// </summary>
        public long GroupSnapshootId { get; set; }

        /// <summary>
        /// 物料id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 客户id
        /// </summary>
        public long CustomerId { get; set; }

        /// <summary>
        /// 出货单Id
        /// </summary>
        public long ShipmentOrderId { get; set; }

        /// <summary>
        /// 出货数量
        /// </summary>
        public decimal ShipmentQty { get; set; }

        /// <summary>
        /// 状态(1-待检验 2-检验中 3-已检验 4-已关闭)
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 是否合格(0-否 1-是)
        /// </summary>
        public bool? IsQualified { get; set; }

        /// <summary>
        /// 备注
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
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }


    }

    /// <summary>
    /// OQC检验单Dto
    /// </summary>
    public record QualOqcOrderDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 检验单号
        /// </summary>
        public string InspectionOrder { get; set; }

        /// <summary>
        /// OQC检验参数组快照Id
        /// </summary>
        public long GroupSnapshootId { get; set; }

        /// <summary>
        /// 物料id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 客户id
        /// </summary>
        public long CustomerId { get; set; }

        /// <summary>
        /// 出货单Id
        /// </summary>
        public long ShipmentOrderId { get; set; }

        /// <summary>
        /// 出货数量
        /// </summary>
        public decimal ShipmentQty { get; set; }

        /// <summary>
        /// 状态(1-待检验 2-检验中 3-已检验 4-已关闭)
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 是否合格(0-否 1-是)
        /// </summary>
        public bool? IsQualified { get; set; }

        /// <summary>
        /// 备注
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
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }


    }

    /// <summary>
    /// OQC检验单分页Dto
    /// </summary>
    public class QualOqcOrderPagedQueryDto : PagerInfo { }

}
