namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// FQC检验单条码记录 查询参数
    /// </summary>
    public class QualFqcOrderSfcQuery
    {
        /// <summary>
        /// 排序(默认为 CreatedOn DESC)
        /// </summary>
        public string Sorting { get; set; } = "CreatedOn DESC";
        
        /// <summary>
        /// 条码
        /// </summary>
        public string? BarCode { get; set; }

        /// <summary>
        /// 批量条码
        /// </summary>
        public IEnumerable<string>? BarCodes { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 检验单表
        /// </summary>
        public long? FQCOrderId { get; set; }

        /// <summary>
        /// 批量检验单表
        /// </summary>
        public IEnumerable<long>? FQCOrderIds { get; set; }
    }
}
