namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// Iqc检验详情 查询参数
    /// </summary>
    public class QualIqcOrderSampleDetailQuery
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
