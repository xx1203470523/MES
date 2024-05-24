namespace Hymson.MES.Data.Repositories.Equipment.Query
{
    /// <summary>
    /// 设备保养任务结果处理 查询参数
    /// </summary>
    public class EquMaintenanceTaskProcessedQuery
    {
        /// <summary>
        /// 主键
        /// </summary>
        public IEnumerable<long>? MaintenanceTaskIds { get; set; }
    }
}
