using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// 检验规则（首检才有） 分页参数
    /// </summary>
    public class QualIpqcInspectionRulePagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
