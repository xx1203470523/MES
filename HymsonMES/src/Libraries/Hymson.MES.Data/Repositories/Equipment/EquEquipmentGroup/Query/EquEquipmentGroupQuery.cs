namespace Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup.Query
{
    /// <summary>
    /// 设备组 查询参数
    /// </summary>
    public class EquEquipmentGroupQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 设备组编码列表
        /// </summary>
        public string[]? EquipmentGroupCodes { get; set; }
    }
}
