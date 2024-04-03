namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// 环境检验参数组快照 查询参数
    /// </summary>
    public class QualEnvParameterGroupSnapshootQuery
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
