/*
 *creator: Karl
 *
 *describe: ESOP    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  zhaoqing
 *build datetime: 2023-11-02 02:39:53
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// ESOP，数据实体对象   
    /// proc_esop
    /// @author zhaoqing
    /// @date 2023-11-02 02:39:53
    /// </summary>
    public class ProcEsopEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 物料Id  proc_material 表Id
        /// </summary>
        public long? MaterialId { get; set; }

       /// <summary>
        /// 工序Id  proc_procedure表 Id
        /// </summary>
        public long? ProcedureId { get; set; }

       /// <summary>
        /// 状态 0-未启用  1-启用
        /// </summary>
        public bool? Status { get; set; }

       
    }
}
