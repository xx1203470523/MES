/*
 *creator: Karl
 *
 *describe: 马威QFC检验附件    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  pengxin
 *build datetime: 2024-07-24 10:03:33
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.QualFqcInspectionMavalAttachment
{
    /// <summary>
    /// 马威QFC检验附件，数据实体对象   
    /// qual_fqc_inspection_maval_attachment
    /// @author pengxin
    /// @date 2024-07-24 10:03:33
    /// </summary>
    public class QualFqcInspectionMavalAttachmentEntity : BaseEntity
    {
        /// <summary>
        /// 马威QFC检验ID；关联qual_fqc_inspection_maval表的ID
        /// </summary>
        public long FqcMavalId { get; set; }

       /// <summary>
        /// 附件ID；关联inte_attachment表的ID
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
