namespace Hymson.MES.Data.Repositories.Manufacture.Query
{
    /// <summary>
    /// Marking拦截记录表 查询参数
    /// </summary>
    public class ManuSfcMarkingInterceptQuery
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
