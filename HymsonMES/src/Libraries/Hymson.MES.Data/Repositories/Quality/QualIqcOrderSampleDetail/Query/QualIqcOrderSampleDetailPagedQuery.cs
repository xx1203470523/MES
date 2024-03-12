using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// Iqc检验详情 分页参数
    /// </summary>
    public class QualIqcOrderSampleDetailPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public long? IQCOrderId { get; set; }

        /// <summary>
        /// 检验类型ID
        /// </summary>
        public long? IQCOrderTypeId { get; set; }

        /// <summary>
        /// 样品条码
        /// </summary>
        public string? Barcode { get; set; }

    }
}
