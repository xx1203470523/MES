namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// FQC检验单样品 查询参数
    /// </summary>
    public class QualFqcOrderSampleQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// FQC检验单Id
        /// </summary>
        public long? FQCOrderId { get; set; }

        /// <summary>
        /// FQC ID Assemble
        /// </summary>
        public IEnumerable<long>? FQCOrderIds { get; set; }

        /// <summary>
        /// 样品条码
        /// </summary>
        public string? Barcode { get; set; }
    }
}
