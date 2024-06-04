using Hymson.MES.Core.Enums.Equipment;

namespace Hymson.MES.Data.Repositories.Equipment.Query
{
    /// <summary>
    /// 设备保养任务操作 查询参数
    /// </summary>
    public class EquMaintenanceTaskOperationQuery
    {
        public IEnumerable<long>? MaintenanceTaskIds { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public EquMaintenanceOperationTypeEnum? OperationType { get; set; }
    }
}
