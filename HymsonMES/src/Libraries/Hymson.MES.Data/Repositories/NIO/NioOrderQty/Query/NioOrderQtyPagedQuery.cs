using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.NIO.Query
{
    /// <summary>
    /// NIO工单数量记录表 分页参数
    /// </summary>
    public class NioOrderQtyPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
