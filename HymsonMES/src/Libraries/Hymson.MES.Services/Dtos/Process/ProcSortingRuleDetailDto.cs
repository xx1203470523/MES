/*
 *creator: Karl
 *
 *describe: 分选规则详情    Dto | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-07-25 03:25:19
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 分选规则详情Dto
    /// </summary>
    public class ProcSortingRuleDetailViewDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// proc_procedure 工序id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 参数Id
        /// </summary>
        public long ParameterId { get; set; }

        /// <summary>
        /// 描述 :参数代码 
        /// 空值 : false  
        /// </summary>
        public string ParameterCode { get; set; }

        /// <summary>
        /// 描述 :参数名称 
        /// 空值 : false  
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// 描述 :参数单位（字典定义） 
        /// 空值 : false  
        /// </summary>
        public ParameterUnitEnum? ParameterUnit { get; set; }

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

    /// <summary>
    /// 分选规则详情新增Dto
    /// </summary>
    public record ProcSortingRuleDetailCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

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
        public bool? MinContainingType { get; set; }

       /// <summary>
        /// 最大值
        /// </summary>
        public decimal? MaxValue { get; set; }

       /// <summary>
        /// 包含最大值类型;1< 2.≥
        /// </summary>
        public bool? MaxContainingType { get; set; }

       /// <summary>
        /// 参数值
        /// </summary>
        public decimal? ParameterValue { get; set; }

       /// <summary>
        /// 等级
        /// </summary>
        public string Rating { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       
    }

    /// <summary>
    /// 分选规则详情更新Dto
    /// </summary>
    public record ProcSortingRuleDetailModifyDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

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
        public bool? MinContainingType { get; set; }

       /// <summary>
        /// 最大值
        /// </summary>
        public decimal? MaxValue { get; set; }

       /// <summary>
        /// 包含最大值类型;1< 2.≥
        /// </summary>
        public bool? MaxContainingType { get; set; }

       /// <summary>
        /// 参数值
        /// </summary>
        public decimal? ParameterValue { get; set; }

       /// <summary>
        /// 等级
        /// </summary>
        public string Rating { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       

    }

    /// <summary>
    /// 分选规则详情Dto
    /// </summary>
    public class ProcSortingRuleDetailQueryDto : PagerInfo
    {
        /// <summary>
        /// 分选规则id
        /// </summary>
        public long? SortingRuleId { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public long? MaterialId { get; set; }
    }
}
