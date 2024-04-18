using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.ManuFeedingTransferRecord.Query
{
    /// <summary>
    /// 上料信息转移记录 分页参数
    /// </summary>
    public class ManuFeedingTransferRecordPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
