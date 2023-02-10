/*
 *creator: Karl
 *
 *describe: 资源配置表 分页查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-10 10:21:26
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 资源配置表 分页参数
    /// </summary>
    public class ProcResourceConfigResPagedQuery : PagerInfo
    {
        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }
    }
}
