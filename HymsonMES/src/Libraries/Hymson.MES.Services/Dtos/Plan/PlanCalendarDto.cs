
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Plan;

/// <summary>
/// <para>@描述：生产日历; 基础数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-15</para>
/// <para><seealso cref="BaseEntityDto">点击查看享元对象</seealso></para>
/// </summary>
public record PlanCalendarDto : BaseEntityDto
{
    /// <summary>
    /// 站点Id
    /// </summary>
    public long? SiteId { get; set; }

    /// <summary>
    /// plan_shift id，班制id
    /// </summary>
    public long? ShiftId { get; set; }

    /// <summary>
    /// 年
    /// </summary>
    public int? Year { get; set; }

    /// <summary>
    /// 月
    /// </summary>
    public int? Month { get; set; }

    /// <summary>
    /// 工作日;工作日，逗号分隔
    /// </summary>
    public string? Workday { get; set; }

    /// <summary>
    /// 状态;0、未启用 1、启用
    /// </summary>
    public YesOrNoEnum? Status { get; set; }

    /// <summary>
    /// 物料组描述
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 明细
    /// </summary>
    public IEnumerable<PlanCalendarDetailDto>? Details { get; set; }
}

/// <summary>
/// <para>@描述：生产日历; 用于更新数据的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-15</para>
/// <para><seealso cref="PlanCalendarDto">点击查看享元对象</seealso></para>
/// </summary>
public record PlanCalendarUpdateDto : PlanCalendarDto
{
    /// <summary>
    /// Id
    /// </summary>
    public long? Id { get; set; }

    /// <summary>
    /// 明细
    /// </summary>
    new public IEnumerable<PlanCalendarDetailUpdateDto>? Details { get; set; }
}

/// <summary>
/// <para>@描述：生产日历; 用于页面展示的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-15</para>
/// <para><seealso cref="PlanCalendarUpdateDto">点击查看享元对象</seealso></para> 
/// </summary>
public record PlanCalendarOutputDto : PlanCalendarUpdateDto
{
    /// <summary>
    /// 创建人
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreatedOn { get; set; }

    /// <summary>
    /// 更新人
    /// </summary>
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdatedOn { get; set; }

    /// <summary>
    /// 明细
    /// </summary>
    new public IEnumerable<PlanCalendarDetailOutputDto> Details { get; set; }

}

/// <summary>
/// <para>@描述：生产日历; 用于删除数据的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-15</para>
/// </summary>
public record PlanCalendarDeleteDto : PlanCalendarDto
{
    /// <summary>
    /// 要删除的组
    /// </summary>
    public IEnumerable<long> Ids { get; set; }
}

/// <summary>
/// <para>@描述：生产日历; 用于条件查询（分页）的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-15</para>
/// <para><seealso cref="PagerInfo">点击查看享元对象</seealso></para>
/// </summary>
public class PlanCalendarPagedQueryDto : PagerInfo
{
    /// <summary>
    /// 年
    /// </summary>
    public int? Year { get; set; }

    /// <summary>
    /// 月
    /// </summary>
    public int? Month { get; set; }
}

/// <summary>
/// <para>@描述：生产日历; 用于条件查询的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-15</para>
/// <para><seealso cref="PagerInfo">点击查看享元对象</seealso></para>
/// </summary>
public class PlanCalendarQueryDto : QueryDtoAbstraction
{
    /// <summary>
    /// 主键
    /// </summary>
    public long? Id { get; set; }
}