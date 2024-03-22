using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// iqc检验单 分页参数
    /// </summary>
    public class QualIqcOrderPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 检验单号
        /// </summary>
        public string? InspectionOrder { get; set; }

        /// <summary>
        /// 状态;1、待检验2、检验中3、已检验4、已关闭
        /// </summary>
        public InspectionStatusEnum? Status { get; set; }

        /// <summary>
        /// 是否免检
        /// </summary>
        public TrueOrFalseEnum? IsExemptInspection { get; set; }

        /// <summary>
        /// 是否合格;0、不合格 1、合格
        /// </summary>
        public TrueOrFalseEnum? IsQualified { get; set; }

        /// <summary>
        /// ID集合（检验单）
        /// </summary>
        public IEnumerable<long>? IQCOrderIds { get; set; }

        /// <summary>
        /// ID集合（物料）
        /// </summary>
        public IEnumerable<long>? MaterialIds { get; set; }

        /// <summary>
        /// ID集合（供应商）
        /// </summary>
        public IEnumerable<long>? SupplierIds { get; set; }

        /// <summary>
        /// 收货单详情Id
        /// </summary>
        public IEnumerable<long>? MaterialReceiptDetailIds { get; set; }

    }
}
