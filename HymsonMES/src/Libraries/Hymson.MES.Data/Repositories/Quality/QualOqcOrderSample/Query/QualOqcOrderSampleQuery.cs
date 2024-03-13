namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// OQC检验单检验样本记录 查询参数
    /// </summary>
    public class QualOqcOrderSampleQuery
    {
        /// <summary>
        /// SiteId
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// OQC检验单Id
        /// </summary>
        public long? OQCOrderId { get; set; }

        /// <summary>
        /// 检验类型ID
        /// </summary>
        public long? OQCOrderTypeId { get; set; }

        /// <summary>
        /// 样品条码
        /// </summary>
        public string? Barcode { get; set; }
    }
}
