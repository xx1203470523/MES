/*
 *creator: Karl
 *
 *describe: 资源作业配置表 分页查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-10 05:26:36
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 资源作业配置表 分页参数
    /// </summary>
    public class ProcResourceConfigJobPagedQuery : PagerInfo
    {
        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }
    }
}
