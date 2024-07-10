using Hymson.Infrastructure;

namespace Hymson.MES.BackgroundServices.NIO
{
    /// <summary>
    /// 蔚来推送项目表 分页参数
    /// </summary>
    public class NioPushItemPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
