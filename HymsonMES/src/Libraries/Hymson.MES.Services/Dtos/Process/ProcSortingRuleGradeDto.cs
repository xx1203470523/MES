/*
 *creator: Karl
 *
 *describe: 档次    Dto | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-07-25 03:34:14
 */

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 档位信息
    /// </summary>
    public class ProcSortingRuleGradeDto
    {
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
        public string Remark { get; set; } = "";

        /// <summary>
        /// 等级
        /// </summary>
        public IEnumerable<string> Ratings { get; set; }
    }
}
