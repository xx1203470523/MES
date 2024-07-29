using Hymson.MES.Core.Enums.Equipment;

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
        /// <summary>
        /// 处理方式
        /// </summary>
        public EquMaintenanceTaskProcessedEnum? HandMethod { get; set; }

        public long? SiteId { get; set; }
    }
}
