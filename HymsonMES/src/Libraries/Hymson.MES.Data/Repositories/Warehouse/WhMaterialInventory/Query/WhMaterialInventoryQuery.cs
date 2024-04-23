namespace Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query
{
    /// <summary>
    /// 物料库存 查询参数
    /// </summary>
    public class WhMaterialInventoryQuery
    {
        /// <summary>
        /// 物料条码
        /// </summary>
        public string MaterialBarCode { get; set; }

        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 物料条码列表
        /// </summary>
        public IEnumerable<string>? MaterialBarCodes { get; set; }
    }
}
