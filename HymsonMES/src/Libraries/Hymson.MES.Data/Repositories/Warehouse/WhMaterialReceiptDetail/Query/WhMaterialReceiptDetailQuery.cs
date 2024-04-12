namespace Hymson.MES.Data.Repositories.Query
{
    /// <summary>
    /// 收料单详情 查询参数
    /// </summary>
    public class WhMaterialReceiptDetailQuery
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 收货单ID
        /// </summary>
        public long? MaterialReceiptId { get; set; }

        /// <summary>
        /// 供应商批次
        /// </summary>
        public string? SupplierBatch { get; set; }

        /// <summary>
        /// 内部
        /// </summary>
        public string? InternalBatch { get; set; }

    }
}
