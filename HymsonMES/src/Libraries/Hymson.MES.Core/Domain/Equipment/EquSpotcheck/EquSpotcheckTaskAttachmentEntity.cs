using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（设备点检任务附件）   
    /// equ_spotcheck_task_attachment
    /// @author JAM
    /// @date 2024-05-22 10:06:44
    /// </summary>
    public class EquSpotcheckTaskAttachmentEntity : BaseEntity
    {
        /// <summary>
        /// 点检任务ID;equ_spotcheck_task表的Id
        /// </summary>
        public long SpotCheckTaskId { get; set; }

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
