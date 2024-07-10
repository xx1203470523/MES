using Hymson.Infrastructure;

namespace Hymson.MES.BackgroundServices.NIO
{
    /// <summary>
    /// 蔚来推送项目关系表 分页参数
    /// </summary>
    public class NioPushItemRelationPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
