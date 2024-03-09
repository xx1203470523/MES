namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// Iqc样本 查询参数
    /// </summary>
    public class QualIqcOrderSampleQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// IQC检验单Id
        /// </summary>
        public long? IQCOrderId { get; set; }

    }
}
