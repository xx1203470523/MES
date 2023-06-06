/*
 *creator: Karl
 *
 *describe: 工序和资源半成品产品设置表 查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-06-05 11:16:51
 */

namespace Hymson.MES.Data.Repositories.Process.ProductSet.Query
{
    /// <summary>
    /// 工序和资源半成品产品设置表 查询参数
    /// </summary>
    public class ProcProductSetQuery
    {
    }


    /// <summary>
    /// 工序/资源产品ID 查询参数
    /// </summary>
    public class GetByProcedureIdAndProductIdQuery
    {
        /// <summary>
        /// 工序/资源ID
        /// </summary>
        public long SetPointId { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

    }
}
