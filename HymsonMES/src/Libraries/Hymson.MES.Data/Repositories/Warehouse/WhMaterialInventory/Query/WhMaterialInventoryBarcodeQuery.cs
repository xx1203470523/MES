namespace Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query
{
    /// <summary>
    /// 
    /// </summary>
    public class WhMaterialInventoryBarCodeQuery
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 条码集合
        /// </summary>
        public string BarCode { get; set; }

    }
}
