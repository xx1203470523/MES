namespace Hymson.MES.Data.Repositories.Warehouse.Query
{
    /// <summary>
    /// 出货单物料详情（外部数据） 查询参数
    /// </summary>
    public class WhShipmentMaterialQuery
    {
        /// <summary>
        /// 出货单Id
        /// </summary>
        public long? ShipmentId { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
    }
}
