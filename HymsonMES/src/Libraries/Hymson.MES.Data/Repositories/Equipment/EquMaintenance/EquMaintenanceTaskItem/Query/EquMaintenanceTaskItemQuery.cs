namespace Hymson.MES.Data.Repositories.Equipment.Query
{
    /// <summary>
    /// 设备保养任务项目 查询参数
    /// </summary>
    public class EquMaintenanceTaskItemQuery
    {
        public long? SiteId { get; set; }
        public long? MaintenanceTaskId { get; set; }
    }
}
