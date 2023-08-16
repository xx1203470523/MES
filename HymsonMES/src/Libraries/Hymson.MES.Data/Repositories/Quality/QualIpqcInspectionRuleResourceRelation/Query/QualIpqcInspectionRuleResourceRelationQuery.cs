namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// 检验规则资源关联（首检才有） 查询参数
    /// </summary>
    public class QualIpqcInspectionRuleResourceRelationQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// IPQC检验项目Id
        /// </summary>
        public long? IpqcInspectionId { get; set; }

        /// <summary>
        /// IPQC检验规则Id
        /// </summary>
        public long? IpqcInspectionRuleId { get; set; }
    }
}
