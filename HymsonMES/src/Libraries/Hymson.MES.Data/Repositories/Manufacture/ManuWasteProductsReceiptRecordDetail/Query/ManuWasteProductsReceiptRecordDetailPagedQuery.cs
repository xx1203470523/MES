using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Manufacture.Query
{
    /// <summary>
    /// 废成品入库明细 分页参数
    /// </summary>
    public class ManuWasteProductsReceiptRecordDetailPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
