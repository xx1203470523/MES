/*
 *creator: Karl
 *
 *describe: 客户维护 分页查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-07-11 09:33:26
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 客户维护 分页参数
    /// </summary>
    public class InteCustomPagedQuery : PagerInfo
    {
        /// <summary>
        /// 客户编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string? Name { get; set; }


        public long SiteId { get; set; }
    }
}
