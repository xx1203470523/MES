namespace Hymson.MES.Data.Repositories.Manufacture.Query
{
    /// <summary>
    /// 条码报废表 查询参数
    /// </summary>
    public class ManuSfcScrapQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码信息表Id
        /// </summary>
        public IEnumerable<long>? SfcinfoIds { get; set; }
    }
}
