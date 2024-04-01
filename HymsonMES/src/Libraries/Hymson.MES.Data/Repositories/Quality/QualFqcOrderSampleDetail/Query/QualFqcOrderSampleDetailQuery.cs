namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// FQC检验单样品检验详情 查询参数
    /// </summary>
    public class QualFqcOrderSampleDetailQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// FQC检验单Id
        /// </summary>
        public long? FQCOrderId { get; set; }
    }
}
