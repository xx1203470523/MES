using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// OQC检验单样本明细附件 分页参数
    /// </summary>
    public class QualOqcOrderSampleDetailAnnexPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
