/*
 *creator: Karl
 *
 *describe: 分选规则详情    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  zhaoqing
 *build datetime: 2023-07-25 03:25:19
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 分选规则详情，数据实体对象   
    /// proc_sorting_rule_detail
    /// @author zhaoqing
    /// @date 2023-07-25 03:25:19
    /// </summary>
    public class ProcSortingRuleDetailEntity : BaseEntity
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
        /// proc_procedure 工序id
        /// </summary>
        public long ProcedureId { get; set; }

       /// <summary>
        /// proc_parameter 参数Id
        /// </summary>
        public long ParameterId { get; set; }

       /// <summary>
        /// 最小值
        /// </summary>
        public decimal? MinValue { get; set; }

       /// <summary>
        /// 包含最小值类型;1< 2.≤
        /// </summary>
        public int MinContainingType { get; set; }

       /// <summary>
        /// 最大值
        /// </summary>
        public decimal? MaxValue { get; set; }

       /// <summary>
        /// 包含最大值类型;1< 2.≥
        /// </summary>
        public int MaxContainingType { get; set; }

       /// <summary>
        /// 参数值
        /// </summary>
        public decimal? ParameterValue { get; set; }

       /// <summary>
        /// 等级
        /// </summary>
        public string Rating { get; set; }

       
    }
}
