using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// IQC检验类型 分页参数
    /// </summary>
    public class QualIqcOrderTypePagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
