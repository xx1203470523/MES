namespace Hymson.MES.Data.Repositories.Equipment.Query
{
    /// <summary>
    /// 设备保养快照任务项目 查询参数
    /// </summary>
    public class EquMaintenanceTaskSnapshotItemQuery
    {
        public long? MaintenanceTaskId { get; set; }
        public IEnumerable<long>? Ids { get; set; }

    }
}
