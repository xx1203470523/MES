namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// FQC检验单条码记录 查询参数
    /// </summary>
    public class QualFqcOrderSfcQuery
    {
        /// <summary>
        /// 排序(默认为 CreatedOn DESC)
        /// </summary>
        public string Sorting { get; set; } = "CreatedOn DESC";

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
    }
}
