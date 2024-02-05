using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.WhWarehouseLocation.Query
{
    /// <summary>
    /// 库位 分页参数
    /// </summary>
    public class WhWarehouseLocationPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// CodeLike
        /// </summary>
        public string? CodeLike { get; set; }

    }
}
