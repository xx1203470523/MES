using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Common.Query
{
    /// <summary>
    /// message_push 分页参数
    /// </summary>
    public class MessagePushPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
