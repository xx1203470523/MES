using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.ManuJzBindRecord.Query
{
    /// <summary>
    /// 极组绑定记录 分页参数
    /// </summary>
    public class ManuJzBindRecordPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
