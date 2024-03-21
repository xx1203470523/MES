using Hymson.MES.Core.Enums.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.ProcSortingRule
{
    /// <summary>
    /// 分选规则详情
    /// </summary>
    public class ProcSortRuleDetailEquDto
    {
        /// <summary>
        /// 分选规则编码
        /// </summary>
        public string SortRuleCode { get; set; } = string.Empty;

        /// <summary>
        /// 分选规则名称
        /// </summary>
        public string SortRuleName { get; set; } = string.Empty;

        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParameterCode { get; set; } = string.Empty;

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName { get; set; } = string.Empty;

        /// <summary>
        /// 最小值
        /// </summary>
        public decimal? MinValue { get; set; }

        /// <summary>
        /// 包含最小值类型;1.小于 2.小于等于
        /// </summary>
        public ContainingTypeEnum? MinContainingType { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public decimal? MaxValue { get; set; }

        /// <summary>
        /// 包含最大值类型;1.小于 2.小于等于
        /// </summary>
        public ContainingTypeEnum? MaxContainingType { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public decimal? ParameterValue { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public string Rating { get; set; } = string.Empty;

        /// <summary>
        /// 序号
        /// </summary>
        public int? Serial { get; set; }
    }
}
