using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.WhShipment.Query
{
    /// <summary>
    /// 出货单 分页参数
    /// </summary>
    public class WhShipmentPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
