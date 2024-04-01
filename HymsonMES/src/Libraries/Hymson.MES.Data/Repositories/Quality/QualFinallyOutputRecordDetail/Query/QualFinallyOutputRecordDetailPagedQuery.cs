using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// 成品条码产出记录明细(FQC生成使用) 分页参数
    /// </summary>
    public class QualFinallyOutputRecordDetailPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
