namespace Hymson.MES.Data.Repositories.WhWarehouseShelf.Query
{
    /// <summary>
    /// 货架 查询参数
    /// </summary>
    public class WhWarehouseShelfQuery
    {
        /// <summary>
        /// ids
        /// </summary>
        public IEnumerable<long>? Ids { get; set; }

        /// <summary>
        /// CodeLike
        /// </summary>
        public string? CodeLike { get; set; }

        /// <summary>
        /// 库区Ids
        /// </summary>
        public IEnumerable<long>? WarehouseRegionIds { get; set; }
    }
}
