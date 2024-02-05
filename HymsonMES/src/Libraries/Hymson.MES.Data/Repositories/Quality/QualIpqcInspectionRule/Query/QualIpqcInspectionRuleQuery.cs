namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// 检验规则（首检才有） 查询参数
    /// </summary>
    public class QualIpqcInspectionRuleQuery
    {
        /// <summary>
        /// IPQC检验项目Id
        /// </summary>
        public long IpqcInspectionId { get; set; }
    }
}
