namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// OQC检验参数组明细 查询参数
    /// </summary>
    public class QualOqcParameterGroupDetailQuery : QueryAbstraction
    {
        public long? SiteId { get; set; }
        /// <summary>
        /// 参数组Id
        /// </summary>
        public long ParameterGroupId { get; set; }
    }
}
