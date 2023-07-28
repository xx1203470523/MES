namespace Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query
{
    /// <summary>
    /// 
    /// </summary>
    public class EquEquipmentQuery
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long SiteId { get; set; }
        /// <summary>
        /// 设备编码
        /// </summary>
        public string[]? EquipmentCodes { get; set; }

    }
}
