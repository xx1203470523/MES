using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（设备保养任务操作）   
    /// equ_maintenance_task_operation
    /// @author JAM
    /// @date 2024-05-23 03:21:32
    /// </summary>
    public class EquMaintenanceTaskOperationEntity : BaseEntity
    {
        /// <summary>
        /// 任务ID;equ_maintenance_task表的Id
        /// </summary>
        public long MaintenanceTaskId { get; set; }

        /// <summary>
        /// 操作类型;1:待处理、2:处理中、3:待审核、4:已关闭
        /// </summary>
        public bool OperationType { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string OperationBy { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperationOn { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        
    }
}
