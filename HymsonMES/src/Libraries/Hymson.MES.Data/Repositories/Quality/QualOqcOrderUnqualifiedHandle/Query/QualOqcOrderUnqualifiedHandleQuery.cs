namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// OQC不合格处理结果 查询参数
    /// </summary>
    public class QualOqcOrderUnqualifiedHandleQuery
    {
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
