namespace Hymson.MES.Data.Repositories.WhWareHouse.Query
{
    /// <summary>
    /// 仓库 查询参数
    /// </summary>
    public class WhWarehouseQuery
    {
        /// <summary>
        /// 仓库Id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 仓库Ids
        /// </summary>
        public IEnumerable<long> Ids { get; set; }
        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WareHouseCode { get; set; }
        // <summary>
        /// 仓库编码列表
        /// </summary>
        public string[] WareHouseCodes { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 仓库编码模糊查询
        /// </summary>
        public string? CodeLike { get; set; }

        /// <summary>
        /// 仓库名称模糊查询
        /// </summary>
        public string? NameLike { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public long? IsDeleted { get; set; }

    }
}
