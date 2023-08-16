using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// 检验规则资源关联（首检才有）新增/更新Dto
    /// </summary>
    public record QualIpqcInspectionRuleResourceRelationSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 检验规则Id;qual_ipqc_inspection_rule 的id
        /// </summary>
        public long IpqcInspectionRuleId { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }


    }

    /// <summary>
    /// 检验规则资源关联（首检才有）Dto
    /// </summary>
    public record QualIpqcInspectionRuleResourceRelationDto : BaseEntityDto
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
        /// 检验规则Id;qual_ipqc_inspection_rule 的id
        /// </summary>
        public long? IpqcInspectionRuleId { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long? ResourceId { get; set; }

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
    /// 检验规则资源关联（首检才有）分页Dto
    /// </summary>
    public class QualIpqcInspectionRuleResourceRelationPagedQueryDto : PagerInfo { }

}
