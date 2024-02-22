namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// IQC检验水平详情 查询参数
    /// </summary>
    public class QualIqcLevelDetailQuery
    {
        /// <summary>
        /// 集合（IQC检验水平Id）
        /// </summary>
        public long[] IqcLevelIds { get; set; }
    }
}
