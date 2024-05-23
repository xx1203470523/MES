using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（设备保养任务项目附件）   
    /// equ_maintenance_task_item_attachment
    /// @author JAM
    /// @date 2024-05-23 03:21:23
    /// </summary>
    public class EquMaintenanceTaskItemAttachmentEntity : BaseEntity
    {
        /// <summary>
        /// 任务ID;equ_maintenance_task表的Id
        /// </summary>
        public long MaintenanceTaskId { get; set; }

        /// <summary>
        /// 任务项目ID;equ_maintenance_task_item表的Id
        /// </summary>
        public long MaintenanceTaskItemId { get; set; }

        /// <summary>
        /// 附件Id;inte_attachment表的Id
        /// </summary>
        public long AttachmentId { get; set; }

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
