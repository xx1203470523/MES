using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Query
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
        /// ID集合（供应商）
        /// </summary>
        public IEnumerable<long>? SupplierIds { get; set; }

    }
}
