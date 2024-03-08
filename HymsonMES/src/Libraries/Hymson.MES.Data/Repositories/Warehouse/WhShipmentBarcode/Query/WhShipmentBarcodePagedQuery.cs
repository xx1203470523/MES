using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Warehouse.Query
{
    /// <summary>
    /// 出货单条码表（外部数据） 分页参数
    /// </summary>
    public class WhShipmentBarcodePagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
