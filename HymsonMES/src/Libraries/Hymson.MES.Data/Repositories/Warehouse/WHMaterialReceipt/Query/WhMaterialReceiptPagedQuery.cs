using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.WHMaterialReceipt.Query
{
    /// <summary>
    /// 物料收货表 分页参数
    /// </summary>
    public class WhMaterialReceiptPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 收货单号
        /// </summary>
        public string? ReceiptNum { get; set; }

        /// <summary>
        /// 供应商Id
        /// </summary>
        public long? SupplierId { get; set; }

    }
}
