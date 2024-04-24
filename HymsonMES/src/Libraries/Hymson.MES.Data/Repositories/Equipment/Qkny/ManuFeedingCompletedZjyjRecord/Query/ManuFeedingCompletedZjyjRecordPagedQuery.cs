using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.ManuFeedingCompletedZjyjRecord.Query
{
    /// <summary>
    /// manu_feeding_completed_zjyj_record 分页参数
    /// </summary>
    public class ManuFeedingCompletedZjyjRecordPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
