using Hymson.Infrastructure;

namespace Hymson.MES.Data.NIO
{
    /// <summary>
    /// 蔚来推送表 分页参数
    /// </summary>
    public class NioPushPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
