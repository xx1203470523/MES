using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Query
{
    /// <summary>
    /// 收料单详情 分页参数
    /// </summary>
    public class WhMaterialReceiptDetailPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
