namespace Hymson.MES.Data.Repositories.WhShipment.Query
{
    /// <summary>
    /// 出货单 查询参数
    /// </summary>
    public class WhShipmentQuery
    {
        /// <summary>
        /// 排序(默认为 CreatedOn DESC)
        /// </summary>
        public string Sorting { get; set; } = "CreatedOn DESC";

        /// <summary>
        /// 出货单Id
        /// </summary>
        public long? ShipmentId { get; set; }

        /// <summary>
        /// 出货单号
        /// </summary>
        public string ShipmentNum { get; set; }

        /// <summary>
        /// Ids
        /// </summary>
        public IEnumerable<long> Ids { get; set; }
    }
}
