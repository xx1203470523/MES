/*
 *creator: Karl
 *
 *describe: esop 文件    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  zhaoqing
 *build datetime: 2023-11-02 02:41:09
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// esop 文件，数据实体对象   
    /// proc_esop_file
    /// @author zhaoqing
    /// @date 2023-11-02 02:41:09
    /// </summary>
    public class ProcEsopFileEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// EsopId  proc_esop 表Id
        /// </summary>
        public long? EsopId { get; set; }

       /// <summary>
        /// 文件id inte_attachment Id
        /// </summary>
        public long AttachmentId { get; set; }

       
    }
}
