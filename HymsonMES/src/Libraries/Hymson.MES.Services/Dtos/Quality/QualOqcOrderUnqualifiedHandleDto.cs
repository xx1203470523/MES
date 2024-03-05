using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// OQC不合格处理结果新增/更新Dto
    /// </summary>
    public record QualOqcOrderUnqualifiedHandleSaveDto : BaseEntityDto
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
        /// FQC检验单Id
        /// </summary>
        public long FQCOrderId { get; set; }

        /// <summary>
        /// 来源系统(1-MES 2-OA)
        /// </summary>
        public bool SourceSystem { get; set; }

        /// <summary>
        /// 不合格处理方式（1-让步 2-挑选 3-返工 4-报废）
        /// </summary>
        public bool? HandMethod { get; set; }

        /// <summary>
        /// 不合格处理人
        /// </summary>
        public string ProcessedBy { get; set; }

        /// <summary>
        /// 不合格处理时间
        /// </summary>
        public DateTime? ProcessedOn { get; set; }

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
    /// OQC不合格处理结果Dto
    /// </summary>
    public record QualOqcOrderUnqualifiedHandleDto : BaseEntityDto
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
        /// FQC检验单Id
        /// </summary>
        public long FQCOrderId { get; set; }

        /// <summary>
        /// 来源系统(1-MES 2-OA)
        /// </summary>
        public bool SourceSystem { get; set; }

        /// <summary>
        /// 不合格处理方式（1-让步 2-挑选 3-返工 4-报废）
        /// </summary>
        public bool? HandMethod { get; set; }

        /// <summary>
        /// 不合格处理人
        /// </summary>
        public string ProcessedBy { get; set; }

        /// <summary>
        /// 不合格处理时间
        /// </summary>
        public DateTime? ProcessedOn { get; set; }

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
    /// OQC不合格处理结果分页Dto
    /// </summary>
    public class QualOqcOrderUnqualifiedHandlePagedQueryDto : PagerInfo { }

}
