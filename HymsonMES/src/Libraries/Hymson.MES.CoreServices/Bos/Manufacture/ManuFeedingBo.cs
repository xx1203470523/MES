namespace Hymson.MES.CoreServices.Bos.Manufacture
{
    /// <summary>
    /// 
    /// </summary>
    public class ManuFeedingMatchBo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 库存条码对应的物料ID
        /// </summary>
        public long InventoryMaterialId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }

    }

}
