using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Manufacture.Query
{
    /// <summary>
    /// 转子装箱记录表 分页参数
    /// </summary>
    public class ManuRotorPackListPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
