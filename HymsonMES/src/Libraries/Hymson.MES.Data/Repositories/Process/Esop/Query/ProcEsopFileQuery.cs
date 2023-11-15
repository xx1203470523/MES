/*
 *creator: Karl
 *
 *describe: esop 文件 查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-11-02 02:41:09
 */

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// esop 文件 查询参数
    /// </summary>
    public class ProcEsopFileQuery
    {
        /// <summary>
        /// EsopId  proc_esop 表Id
        /// </summary>
        public long? EsopId { get; set; }

        /// <summary>
        /// EsopIds
        /// </summary>
        public IEnumerable<long>? EsopIds { get; set; }
    }
}
