namespace Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query
{
    /// <summary>
    /// 
    /// </summary>
    public class WhMaterialInventoryBarCodesQuery
    {
        /// <summary>
        /// 条码集合
        /// </summary>
        public IEnumerable<string> BarCodes { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }
    }
}
