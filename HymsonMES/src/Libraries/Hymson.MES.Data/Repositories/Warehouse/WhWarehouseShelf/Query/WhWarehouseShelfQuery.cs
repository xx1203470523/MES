namespace Hymson.MES.Data.Repositories.WhWarehouseShelf.Query
{
    /// <summary>
    /// 货架 查询参数
    /// </summary>
    public class WhWarehouseShelfQuery
    {
        /// <summary>
        /// id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// ids
        /// </summary>
        public IEnumerable<long>? Ids { get; set; }

        /// <summary>
        /// 货架编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// CodeLike
        /// </summary>
        public string? CodeLike { get; set; }

        /// <summary>
        /// 库区Ids
        /// </summary>
        public IEnumerable<long>? WarehouseRegionIds { get; set; }

        /// <summary>
        /// 仓库Ids
        /// </summary>
        public IEnumerable<long>? WarehouseIds { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long? SiteId { get; set; }
    }
}
