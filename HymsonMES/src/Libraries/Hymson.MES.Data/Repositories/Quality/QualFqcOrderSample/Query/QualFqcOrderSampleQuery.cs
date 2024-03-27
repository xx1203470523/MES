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
        /// 检验类型ID
        /// </summary>
        public long? FQCOrderTypeId { get; set; }

        /// <summary>
        /// 样品条码
        /// </summary>
        public string? Barcode { get; set; }
    }
}
