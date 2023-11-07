/*
 *creator: Karl
 *
 *describe: ESOP 查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-11-02 02:39:53
 */

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// ESOP 查询参数
    /// </summary>
    public class ProcEsopQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public long? MaterialId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long? ProcedureId { get; set;}
    }
}
