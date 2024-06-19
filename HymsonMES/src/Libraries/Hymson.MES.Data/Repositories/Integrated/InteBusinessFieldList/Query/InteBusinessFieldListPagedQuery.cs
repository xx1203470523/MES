using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Integrated.Query
{
    /// <summary>
    /// 字段定义列表数据 分页参数
    /// </summary>
    public class InteBusinessFieldListPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
