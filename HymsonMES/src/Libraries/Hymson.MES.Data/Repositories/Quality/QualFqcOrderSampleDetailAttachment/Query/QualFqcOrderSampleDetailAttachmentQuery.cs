namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// FQC检验单样本明细附件 查询参数
    /// </summary>
    public class QualFqcOrderSampleDetailAttachmentQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// IQC检验单Id
        /// </summary>
        public long? FQCOrderId { get; set; }
    }
}
