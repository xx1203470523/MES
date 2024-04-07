using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Equipment.Query
{
    /// <summary>
    /// 点检任务 分页参数
    /// </summary>
    public class EquInspectionTaskPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
