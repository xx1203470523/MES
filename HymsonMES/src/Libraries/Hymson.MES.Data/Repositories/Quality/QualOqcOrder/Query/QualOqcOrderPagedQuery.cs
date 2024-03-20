using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// OQC检验单 分页参数
    /// </summary>
    public class QualOqcOrderPagedQuery : PagerInfo
    {
        /// <summary>
        /// Ids
        /// </summary>
        public IEnumerable<long>? Ids { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 检验单号模糊条件
        /// </summary>
        public string? InspectionOrderLike { get; set; }

        /// <summary>
        /// 状态(1-待检验 2-检验中 3-已检验 4-已关闭)
        /// </summary>
        public InspectionStatusEnum? Status { get; set; }

        /// <summary>
        /// 物料Ids
        /// </summary>
        public IEnumerable<long>? MaterialIds { get; set; }

        /// <summary>
        /// 客户id组
        /// </summary>
        public IEnumerable<long>? CustomerIds { get; set; }

        /// <summary>
        /// 出货单明细Id组
        /// </summary>
        public IEnumerable<long>? ShipmentMaterialIds { get; set; }

        /// <summary>
        /// 是否合格(0-否 1-是)
        /// </summary>
        public TrueOrFalseEnum? IsQualified { get; set; }
    }
}
