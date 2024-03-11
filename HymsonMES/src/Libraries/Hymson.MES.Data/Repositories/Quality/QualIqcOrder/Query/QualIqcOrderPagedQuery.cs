using Hymson.Infrastructure;
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
        /// 物料ID
        /// </summary>
        public IEnumerable<long>? MaterialIds { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public IEnumerable<long>? SupplierIds { get; set; }

        /// <summary>
        /// 收货单详情Id
        /// </summary>
        public IEnumerable<long>? MaterialReceiptDetailIds { get; set; }

        /// <summary>
        /// 状态;1、待检验2、检验中3、已检验4、已关闭
        /// </summary>
        public InspectionStatusEnum? Status { get; set; }

    }
}
