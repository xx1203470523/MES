using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.ManuFeedingNoProductionRecord.Query
{
    /// <summary>
    /// 设备投料非生产投料(洗罐子) 分页参数
    /// </summary>
    public class ManuFeedingNoProductionRecordPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
