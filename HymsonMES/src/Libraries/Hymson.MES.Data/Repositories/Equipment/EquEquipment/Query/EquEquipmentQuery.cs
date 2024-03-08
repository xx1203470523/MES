namespace Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query
{
    /// <summary>
    /// 
    /// </summary>
    public class EquEquipmentQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 设备编码列表
        /// </summary>
        public string[]? EquipmentCodes { get; set; }
    }
}
