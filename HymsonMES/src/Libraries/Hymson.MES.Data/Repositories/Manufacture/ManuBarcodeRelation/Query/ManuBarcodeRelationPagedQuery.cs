using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Manufacture.Query
{
    /// <summary>
    /// 条码关系表 分页参数
    /// </summary>
    public class ManuBarcodeRelationPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
