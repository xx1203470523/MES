using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.NioPushCollection.Query
{
    /// <summary>
    /// NIO推送参数 分页参数
    /// </summary>
    public class NioPushCollectionPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
