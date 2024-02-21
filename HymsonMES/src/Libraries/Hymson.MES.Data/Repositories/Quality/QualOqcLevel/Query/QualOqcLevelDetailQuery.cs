namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// OQC检验水平详情 查询参数
    /// </summary>
    public class QualOqcLevelDetailQuery
    {
        /// <summary>
        /// 集合（OQC检验水平Id）
        /// </summary>
        public long[] OqcLevelIds { get; set; }
    }
}
