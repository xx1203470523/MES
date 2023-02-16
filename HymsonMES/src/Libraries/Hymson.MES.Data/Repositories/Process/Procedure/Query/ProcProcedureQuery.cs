/*
 *creator: Karl
 *
 *describe: 工序表 查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-13 09:06:05
 */

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 工序表 查询参数
    /// </summary>
    public class ProcProcedureQuery
    {
        /// <summary>
        /// 工序编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 所属站点代码
        /// </summary>
        public string SiteCode { get; set; }
    }
}
