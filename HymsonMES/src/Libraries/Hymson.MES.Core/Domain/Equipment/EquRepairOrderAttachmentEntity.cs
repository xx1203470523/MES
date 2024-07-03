/*
 *creator: Karl
 *
 *describe: 设备维修附件    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  pengxin
 *build datetime: 2024-07-01 04:52:14
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Equipment;

namespace Hymson.MES.Core.Domain.EquRepairOrderAttachment
{
    /// <summary>
    /// 设备维修附件，数据实体对象   
    /// equ_repair_order_attachment 
    /// @author pengxin
    /// @date 2024-07-01 04:52:14
    /// </summary>
    public class EquRepairOrderAttachmentEntity : BaseEntity
    {
        /// <summary>
        /// 附件类型；1报修2维修
        /// </summary>
        public EquRepairOrderAttachmentTypeEnum AttachmentType { get; set; }

        /// <summary>
        /// 设备维修ID；关联equ_repair_order表的ID
        /// </summary>
        public long RepairOrderId { get; set; }

        /// <summary>
        /// 附件ID；关联inte_attachment表的ID
        /// </summary>
        public long AttachmentId { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }


    }
}
