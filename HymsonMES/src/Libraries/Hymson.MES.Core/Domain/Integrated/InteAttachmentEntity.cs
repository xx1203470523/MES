using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 数据实体（附件表）   
    /// inte_attachment
    /// @author Czhipu
    /// @date 2023-08-16 10:13:04
    /// </summary>
    public class InteAttachmentEntity : BaseEntity
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 文件路径
        /// </summary>
        public string Path { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

       /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
