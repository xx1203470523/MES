using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（设备保养任务附件）   
    /// equ_maintenance_task_attachment
    /// @author JAM
    /// @date 2024-05-23 03:21:02
    /// </summary>
    public class EquMaintenanceTaskAttachmentEntity : BaseEntity
    {
        /// <summary>
        /// 任务ID;equ_maintenance_task表的Id
        /// </summary>
        public long MaintenanceTaskId { get; set; }

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
