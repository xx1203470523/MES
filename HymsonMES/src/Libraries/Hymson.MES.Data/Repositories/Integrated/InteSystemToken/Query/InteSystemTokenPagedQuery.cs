/*
 *creator: Karl
 *
 *describe: 系统Token 分页查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-06-15 02:09:57
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 系统Token 分页参数
    /// </summary>
    public class InteSystemTokenPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 系统编码
        /// </summary>
        public string? SystemCode { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        public string? SystemName { get; set; }

    }
}
