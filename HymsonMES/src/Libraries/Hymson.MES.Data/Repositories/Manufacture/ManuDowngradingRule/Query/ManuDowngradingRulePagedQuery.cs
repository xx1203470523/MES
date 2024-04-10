/*
 *creator: Karl
 *
 *describe: 降级规则 分页查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-08-07 02:00:57
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 降级规则 分页参数
    /// </summary>
    public class ManuDowngradingRulePagedQuery : PagerInfo
    {
        public long SiteId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 或编码
        /// </summary>
        public string? OrCode { get; set; }

        /// <summary>
        /// 或名称
        /// </summary>
        public string? OrName { get; set; }
    }
}
