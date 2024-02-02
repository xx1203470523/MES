using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Plan.Query
{
    /// <summary>
    /// 班制 分页参数
    /// </summary>
    public class PlanShiftPagedQuery : PagerInfo
    {

        public string? Code { get; set; }

        public string? Name { get; set; }
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
