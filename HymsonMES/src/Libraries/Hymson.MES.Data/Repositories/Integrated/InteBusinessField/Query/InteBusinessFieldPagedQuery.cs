using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Integrated.Query
{
    /// <summary>
    /// 字段定义 分页参数
    /// </summary>
    public class InteBusinessFieldPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
