using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Integrated.Query
{
    /// <summary>
    /// 字段分配管理详情 分页参数
    /// </summary>
    public class InteBusinessFieldDistributeDetailsPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }

    public class InteBusinessFieldDistributeDetailBusinessFieldIdIdsQuery
    {
        public long[] BusinessFieldIds { get; set; }

        public long SiteId { get; set; }
    }
}
