/*
 *creator: Karl
 *
 *describe: 资源配置打印机 分页查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-09 04:14:52
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 资源配置打印机 分页参数
    /// </summary>
    public class ProcResourceConfigPrintPagedQuery : PagerInfo
    {
        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }
    }
}
