/*
 *creator: Karl
 *
 *describe: 分选规则 分页查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-07-25 03:24:54
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 分选规则 分页参数
    /// </summary>
    public class ProcSortingRulePagedQuery : PagerInfo
    {
        /// <summary>
        /// 工序编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }


        /// <summary>
        /// 版本
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; } = 0;

        /// <summary>
        /// 物料id
        /// </summary>
        public long? MaterialId { get; set; }

        /// <summary>
        /// 物料编码组
        /// </summary>
        public IEnumerable<long>? MaterialIds { get; set; }
    }
}
