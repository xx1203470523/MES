using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// OQC检验单检验样本明细 分页参数
    /// </summary>
    public class QualOqcOrderSampleDetailPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// OQC样本Ids
        /// </summary>
        public IEnumerable<long>? OQCOrderSampleIds { get; set; }

        /// <summary>
        /// OQC检验参数组明细快照Ids
        /// </summary>
        public IEnumerable<long>? GroupDetailSnapshootIds { get; set; }

        /// <summary>
        /// OQCOrderId
        /// </summary>
        public long? OQCOrderId { get; set; }

        /// <summary>
        /// 是否合格(0-否 1-是)
        /// </summary>
        public TrueOrFalseEnum? IsQualified { get; set; }
    }
}
