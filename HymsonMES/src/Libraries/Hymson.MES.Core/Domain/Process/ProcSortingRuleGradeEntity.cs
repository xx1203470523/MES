/*
 *creator: Karl
 *
 *describe: 档次    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  zhaoqing
 *build datetime: 2023-07-25 03:34:14
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 档次，数据实体对象   
    /// proc_sorting_rule_grade
    /// @author zhaoqing
    /// @date 2023-07-25 03:34:14
    /// </summary>
    public class ProcSortingRuleGradeEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// proc_sorting_rules 分选规则id
        /// </summary>
        public long SortingRuleId { get; set; }

        /// <summary>
        /// 档位
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; } = "";

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority { get; set; }
    }
}
