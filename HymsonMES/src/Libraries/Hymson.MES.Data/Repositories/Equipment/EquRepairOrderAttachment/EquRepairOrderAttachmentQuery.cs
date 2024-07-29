/*
 *creator: Karl
 *
 *describe: 设备维修附件 查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-07-01 04:52:14
 */

using Hymson.MES.Core.Enums.Equipment;

namespace Hymson.MES.Data.Repositories.EquRepairOrderAttachment
{
    /// <summary>
    /// 设备维修附件 查询参数
    /// </summary>
    public class EquRepairOrderAttachmentQuery
    {
        /// <summary>
        /// 维修单Id
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        ///附件类型
        /// </summary>
        public EquRepairOrderAttachmentTypeEnum AttachmentType { get; set; }
    }
}
