namespace Hymson.MES.Data.Repositories.Equipment.Query
{
    /// <summary>
    /// 设备保养任务项目附件 查询参数
    /// </summary>
    public class EquMaintenanceTaskItemAttachmentQuery
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long? SiteId { get; set; }
        public long? MaintenanceTaskId { get; set; }
    }
}
