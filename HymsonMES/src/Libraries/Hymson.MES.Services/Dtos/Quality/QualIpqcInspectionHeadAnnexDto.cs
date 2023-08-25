using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// 首检附件新增/更新Dto
    /// </summary>
    public record QualIpqcInspectionHeadAnnexSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 附件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 附件路径
        /// </summary>
        public string Path { get; set; }
    }

    /// <summary>
    /// 首检附件Dto
    /// </summary>
    public record QualIpqcInspectionHeadAnnexDto : BaseEntityDto
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
        /// 首检检验单Id
        /// </summary>
        public long IpqcInspectionHeadId { get; set; }

        /// <summary>
        /// 附件id
        /// </summary>
        public long AnnexId { get; set; }

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


    }

    /// <summary>
    /// 首检附件分页Dto
    /// </summary>
    public class QualIpqcInspectionHeadAnnexPagedQueryDto : PagerInfo { }

}
