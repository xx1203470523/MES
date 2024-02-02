
using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Plan;

/// <summary>
/// <para>@描述：生产日历详情; 基础数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-15</para>
/// <para><seealso cref="BaseEntityDto">点击查看享元对象</seealso></para>
/// </summary>
public record PlanCalendarDetailDto : BaseEntityDto
{
    /// <summary>
    /// 站点Id
    /// </summary>
    public long? SiteId { get; set; }

    /// <summary>
    /// 生产日历Id
    /// </summary>
    public long? PlanCalendarId { get; set; }

    /// <summary>
    /// 生产日
    /// </summary>
    public int? Day { get; set; }

    /// <summary>
    /// plan_shift的id
    /// </summary>
    public long? ShiftId { get; set; }

    /// <summary>
    /// 物料组描述
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// <para>@描述：生产日历详情; 用于更新数据的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-15</para>
/// <para><seealso cref="PlanCalendarDetailDto">点击查看享元对象</seealso></para>
/// </summary>
public record PlanCalendarDetailUpdateDto : PlanCalendarDetailDto
{
    /// <summary>
    /// Id
    /// </summary>
    public long? Id { get; set; }
}

/// <summary>
/// <para>@描述：生产日历详情; 用于页面展示的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-15</para>
/// <para><seealso cref="PlanCalendarDetailUpdateDto">点击查看享元对象</seealso></para>
/// </summary>
public record PlanCalendarDetailOutputDto : PlanCalendarDetailUpdateDto
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

}

/// <summary>
/// <para>@描述：生产日历详情; 用于删除数据的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-15</para>
/// </summary>
public record PlanCalendarDetailDeleteDto : PlanCalendarDetailDto
{
    /// <summary>
    /// 要删除的组
    /// </summary>
    public IEnumerable<long> Ids { get; set; }
}

/// <summary>
/// <para>@描述：生产日历详情; 用于条件查询（分页）的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-15</para>
/// <para><seealso cref="PagerInfo">点击查看享元对象</seealso></para>
/// </summary>
public class PlanCalendarDetailPagedQueryDto : PagerInfo
{
}

/// <summary>
/// <para>@描述：生产日历详情; 用于条件查询（分页）的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-15</para>
/// <para><seealso cref="PagerInfo">点击查看享元对象</seealso></para>
/// </summary>
public class PlanCalendarDetailQueryDto : QueryDtoAbstraction
{
}