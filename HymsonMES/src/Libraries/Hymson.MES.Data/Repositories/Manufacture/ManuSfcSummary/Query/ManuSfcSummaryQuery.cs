namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码工序生产汇总表 查询参数
    /// </summary>
    public class ManuSfcSummaryQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }


        /// <summary>
        /// 条码
        /// </summary>
        public string? SFC { get; set; }

    }
}
