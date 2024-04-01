namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// 成品条码产出记录明细(FQC生成使用) 查询参数
    /// </summary>
    public class QualFinallyOutputRecordDetailQuery
    {
        /// <summary>
        /// 排序(默认为 CreatedOn DESC)
        /// </summary>
        public string Sorting { get; set; } = "CreatedOn DESC";

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 产出记录Ids
        /// </summary>
        public IEnumerable<long>? OutputRecordIds { get; set; }
    }
}
