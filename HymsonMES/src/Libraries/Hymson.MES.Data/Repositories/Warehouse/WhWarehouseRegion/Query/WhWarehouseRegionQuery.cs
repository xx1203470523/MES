namespace Hymson.MES.Data.Repositories.WhWarehouseRegion.Query
{
    /// <summary>
    /// 库区 查询参数
    /// </summary>
    public class WhWarehouseRegionQuery
    {
        /// <summary>
        /// 库区编码模糊查询
        /// </summary>
        public string? CodeLike { get; set; }

        /// <summary>
        /// 仓库Ids
        /// </summary>
        public IEnumerable<long> WarehouseIds { get; set; }
    }
}
