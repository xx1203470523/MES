using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（设备保养任务项目）   
    /// equ_maintenance_task_item
    /// @author JAM
    /// @date 2024-05-23 03:21:14
    /// </summary>
    public class EquMaintenanceTaskItemEntity : BaseEntity
    {
        /// <summary>
        /// 任务ID;equ_maintenance_task表的Id
        /// </summary>
        public long MaintenanceTaskId { get; set; }

        /// <summary>
        /// 项目快照ID;equ_maintenance_item_snapshot表的Id
        /// </summary>
        public long MaintenanceItemSnapshotId { get; set; }

        /// <summary>
        /// 检验值
        /// </summary>
        public string InspectionValue { get; set; }

        /// <summary>
        /// 是否合格;(0-否 1-是)
        /// </summary>
        public bool IsQualified { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        
    }
}
