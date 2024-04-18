using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.ManuJzBind.Query
{
    /// <summary>
    /// 极组绑定 分页参数
    /// </summary>
    public class ManuJzBindPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
