namespace Hymson.MES.Data.Repositories.Warehouse.Query
{
    /// <summary>
    /// 出货单条码表（外部数据） 查询参数
    /// </summary>
    public class WhShipmentBarcodeQuery
    {
        /// <summary>
        /// 出货单详情Ids
        /// </summary>
        public IEnumerable<long>? ShipmentDetailIds { get; set; }

        /// <summary>
        /// 出货条码
        /// </summary>
        public string? BarCode { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }
    }
}
