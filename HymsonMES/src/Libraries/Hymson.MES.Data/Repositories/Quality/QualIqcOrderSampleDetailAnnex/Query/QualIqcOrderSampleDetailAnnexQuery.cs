namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// 首检附件 查询参数
    /// </summary>
    public class QualIqcOrderSampleDetailAnnexQuery
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
