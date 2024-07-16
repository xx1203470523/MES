namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// iqc检验单（新） 查询参数
    /// </summary>
    public class QualIqcOrderLiteDetailQuery
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// IQC检验单Id
        /// </summary>
        public long? IQCOrderId { get; set; }

    }
}
