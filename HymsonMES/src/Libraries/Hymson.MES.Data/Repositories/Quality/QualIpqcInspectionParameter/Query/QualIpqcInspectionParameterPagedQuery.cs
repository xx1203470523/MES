using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// IPQC检验项目参数 分页参数
    /// </summary>
    public class QualIpqcInspectionParameterPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
