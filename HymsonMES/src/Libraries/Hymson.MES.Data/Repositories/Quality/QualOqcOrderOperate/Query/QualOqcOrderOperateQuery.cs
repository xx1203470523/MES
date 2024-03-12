namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// OQC检验单操作记录 查询参数
    /// </summary>
    public class QualOqcOrderOperateQuery
    {
        /// <summary>
        /// OQC检验单Id
        /// </summary>
        public long? OQCOrderId { get; set; }

        /// <summary>
        /// OQC检验单Ids
        /// </summary>
        public IEnumerable<long>? OQCOrderIds { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
    }
}
