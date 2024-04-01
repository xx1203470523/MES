using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// 成品条码产出记录(FQC生成使用) 分页参数
    /// </summary>
    public class QualFinallyOutputRecordPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
