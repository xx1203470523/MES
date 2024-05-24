namespace Hymson.MES.Data.Repositories.Equipment.EquMaintenance.EquMaintenanceItem.Query
{
    /// <summary>
    /// 设备保养项目 查询参数
    /// </summary>
    public class EquMaintenanceItemQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long? SiteId { get; set; }
        public string? Code { get; set; }
    }
}
