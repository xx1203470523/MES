using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（设备保养任务结果处理）   
    /// equ_maintenance_task_processed
    /// @author JAM
    /// @date 2024-05-23 03:21:40
    /// </summary>
    public class EquMaintenanceTaskProcessedEntity : BaseEntity
    {
        /// <summary>
        /// 任务ID;equ_maintenance_task表的Id
        /// </summary>
        public long MaintenanceTaskId { get; set; }

        /// <summary>
        /// 不合格处理方式;1-通过；2-不通过
        /// </summary>
        public bool? HandMethod { get; set; }

        /// <summary>
        /// 处理人
        /// </summary>
        public string ProcessedBy { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? ProcessedOn { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        
    }
}
