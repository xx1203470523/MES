namespace Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Query
{
    /// <summary>
    /// 查询参数
    /// </summary>
    public class ManuFeedingQuery
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public IEnumerable<long> MaterialIds { get; set; }
    }
}
