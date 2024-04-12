namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// OQC检验单样本明细附件 查询参数
    /// </summary>
    public class QualOqcOrderSampleDetailAnnexQuery
    {
        /// <summary>
        /// SiteId
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// OQCOrderId
        /// </summary>
        public long? OQCOrderId { get; set; }
    }
}
