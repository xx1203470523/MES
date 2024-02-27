using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Plan;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Dtos.Plan
{
    /// <summary>
    /// 班制新增/更新Dto
    /// </summary>
    public record PlanShiftSaveDto : BaseEntityDto
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
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 物料组描述
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        //public string CreatedBy { get; set; }

        ///// <summary>
        ///// 创建时间
        ///// </summary>
        //public DateTime CreatedOn { get; set; }

        ///// <summary>
        ///// 更新人
        ///// </summary>
        //public string UpdatedBy { get; set; }

        ///// <summary>
        ///// 更新时间
        ///// </summary>
        //public DateTime UpdatedOn { get; set; }

        ///// <summary>
        ///// 删除标识
        ///// </summary>
        //public long IsDeleted { get; set; }

        public IList<PlanShiftDetailModifyDto> PlanShiftDetailList { get; set; }


    }

    /// <summary>
    /// 班制Dto
    /// </summary>
    public record PlanShiftDto : BaseEntityDto
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
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述 :状态 
        /// 空值 : false  
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 物料组描述
        /// </summary>
        public string Remark { get; set; } = "";

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


    public record PlanShiftModifyDto : BaseEntityDto
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
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 物料组描述
        /// </summary>
        public string Remark { get; set; } = "";

        public IList<PlanShiftDetailModifyDto> PlanShiftDetailList { get; set; }


    }

    /// <summary>
    /// 详细表
    /// </summary>
    public record PlanShiftDetailModifyDto: BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        //public long Id { get; set; }

        /// <summary>
        /// 主表Id
        /// </summary>
        public long ShfitId { get; set; }

        /// <summary>
        /// 班次类型;1、早班 2、中班 3、晚班
        /// </summary>
        public InteShiftTypeEnum ShiftType { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 是否跨天;0、否  1、 是
        /// </summary>
        public bool IsDaySpan { get; set; }

        /// <summary>
        /// 是否加班;0、否  1、 是
        /// </summary>
        public bool IsOverTime { get; set; }

        /// <summary>
        /// 物料组描述
        /// </summary>
        public string? Remark { get; set; }
    }

    /// <summary>
    /// 班制分页Dto
    /// </summary>
    public class PlanShiftPagedQueryDto : PagerInfo
    {
        public string? Code { get; set; }

        public string? Name { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }
    }

}
