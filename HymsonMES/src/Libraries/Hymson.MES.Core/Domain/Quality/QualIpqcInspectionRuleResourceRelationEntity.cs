using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（检验规则资源关联（首检才有））   
    /// qual_ipqc_inspection_rule_resource_relation
    /// @author xiaofei
    /// @date 2023-08-08 11:33:08
    /// </summary>
    public class QualIpqcInspectionRuleResourceRelationEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// IPQC检验项目Id qual_ipqc_inspection 的id
        /// </summary>
        public long IpqcInspectionId { get; set; }

        /// <summary>
        /// 检验规则Id;qual_ipqc_inspection_rule 的id
        /// </summary>
        public long IpqcInspectionRuleId { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }


    }
}
