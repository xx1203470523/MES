namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// FQC检验参数组明细 查询参数
    /// </summary>
    public class QualFqcParameterGroupDetailQuery:QueryAbstraction
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
        /// FQC检验参数组Id
        /// </summary>
        public long? ParameterGroupId { get; set; }

        /// <summary>
        /// FQC检验参数组Ids
        /// </summary>
        public IEnumerable<long>? ParameterGroupIds { get; set; }

    }
}
