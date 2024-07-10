using Hymson.Infrastructure;

namespace Hymson.MES.BackgroundServices.NIO
{
    /// <summary>
    /// 蔚来推送开关 分页参数
    /// </summary>
    public class NioPushSwitchPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
