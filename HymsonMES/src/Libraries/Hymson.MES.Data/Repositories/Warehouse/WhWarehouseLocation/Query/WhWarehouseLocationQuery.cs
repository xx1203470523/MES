namespace Hymson.MES.Data.Repositories.WhWarehouseLocation.Query
{
    /// <summary>
    /// 库位 查询参数
    /// </summary>
    public class WhWarehouseLocationQuery: QueryAbstraction
    {
        /// <summary>
        /// 货架Id
        /// </summary>
        public long? WarehouseShelfId { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 货架Ids
        /// </summary>
        public IEnumerable<long>? WarehouseShelfIds { get; set; }
    }
}
