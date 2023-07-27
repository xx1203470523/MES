/*
 *creator: Karl
 *
 *describe: 档位详情    Dto | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-07-25 03:34:23
 */

using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 档位详情Dto
    /// </summary>
    public record ProcSortingRuleGradeDetailsDto : BaseEntityDto
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
    /// 档位详情新增Dto
    /// </summary>
    public record ProcSortingRuleGradeDetailsCreateDto : BaseEntityDto
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
    /// 档位详情更新Dto
    /// </summary>
    public record ProcSortingRuleGradeDetailsModifyDto : BaseEntityDto
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
    /// 档位详情分页Dto
    /// </summary>
    public class ProcSortingRuleGradeDetailsPagedQueryDto : PagerInfo
    {
    }
}
