namespace Hymson.MES.Data.Repositories.Query
{
    /// <summary>
    /// 物料收货表 查询参数
    /// </summary>
    public class WhMaterialReceiptQuery
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 收货单号
        /// </summary>
        public string? ReceiptNum { get; set; }

    }
}
