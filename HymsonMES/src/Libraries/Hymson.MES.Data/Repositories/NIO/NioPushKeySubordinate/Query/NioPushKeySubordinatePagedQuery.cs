using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.NIO.Query
{
    /// <summary>
    /// 物料及其关键下级件信息表 分页参数
    /// </summary>
    public class NioPushKeySubordinatePagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
