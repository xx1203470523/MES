/*
 *creator: Karl
 *
 *describe: 档位详情    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  zhaoqing
 *build datetime: 2023-07-25 03:34:23
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 档位详情，数据实体对象   
    /// proc_sorting_rule_grade_details
    /// @author zhaoqing
    /// @date 2023-07-25 03:34:23
    /// </summary>
    public class ProcSortingRuleGradeDetailsEntity : BaseEntity
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
        /// proc_sorting_rule_grade分选规则档位id
        /// </summary>
        public long SortingRuleGradeId { get; set; }

       /// <summary>
        /// sorting_rule_detail 分选规则详情Id
        /// </summary>
        public long SortingRuleDetailId { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

       
    }
}
