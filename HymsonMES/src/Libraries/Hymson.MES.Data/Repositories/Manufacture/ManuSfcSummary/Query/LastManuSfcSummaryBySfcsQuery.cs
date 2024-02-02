namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 查询条码最后数据
    /// </summary>
    public class LastManuSfcSummaryBySfcsQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码集合
        /// </summary>
        public IEnumerable<string> Sfcs { get; set; }
    }
}
