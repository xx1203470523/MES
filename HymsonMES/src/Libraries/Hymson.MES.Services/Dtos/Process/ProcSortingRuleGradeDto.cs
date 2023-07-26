/*
 *creator: Karl
 *
 *describe: 档次    Dto | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-07-25 03:34:14
 */

using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 档次Dto
    /// </summary>
    public record ProcSortingRuleGradeDto : BaseEntityDto
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
        /// 档位
        /// </summary>
        public string Grade { get; set; }

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
    /// 档次新增Dto
    /// </summary>
    public record ProcSortingRuleGradeCreateDto : BaseEntityDto
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
        /// 档位
        /// </summary>
        public string Grade { get; set; }

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
    /// 档次更新Dto
    /// </summary>
    public record ProcSortingRuleGradeModifyDto : BaseEntityDto
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
        /// 档位
        /// </summary>
        public string Grade { get; set; }

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
    /// 档次分页Dto
    /// </summary>
    public class ProcSortingRuleGradePagedQueryDto : PagerInfo
    {
    }
}
