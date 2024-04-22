using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.AgvTaskRecord.Query
{
    /// <summary>
    /// AGV任务记录表 分页参数
    /// </summary>
    public class AgvTaskRecordPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
