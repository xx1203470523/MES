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
        public long SiteId { get; set; }

    }
}
