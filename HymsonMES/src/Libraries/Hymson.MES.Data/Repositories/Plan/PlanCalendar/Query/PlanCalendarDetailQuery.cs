
using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Plan;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：查询对象</para>
/// <para>@描述：生产日历详情;标准查询对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-30</para>
/// </summary>
public class PlanCalendarDetailQuery : QueryAbstraction
{

    /// <summary>
    /// 主键
    /// </summary>
    public long? Id { get; set; }

    /// <summary>
    /// 主键组
    /// </summary>
    public IEnumerable<long>? Ids { get; set; }


    /// <summary>
    /// 站点Id
    /// </summary>
    public long? SiteId { get; set; }

    /// <summary>
    /// 站点Id组
    /// </summary>
    public IEnumerable<long>? SiteIds { get; set; }


    /// <summary>
    /// 生产日历Id
    /// </summary>
    public long? PlanCalendarId { get; set; }

    /// <summary>
    /// 生产日历Id组
    /// </summary>
    public IEnumerable<long>? PlanCalendarIds { get; set; }


    /// <summary>
    /// plan_shift id，班制id
    /// </summary>
    public long? ShiftId { get; set; }

    /// <summary>
    /// plan_shift id，班制id组
    /// </summary>
    public IEnumerable<long>? ShiftIds { get; set; }


    /// <summary>
    /// 物料组描述
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 物料组描述模糊条件
    /// </summary>
    public string? RemarkLike { get; set; }


    /// <summary>
    /// 创建人
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// 创建人模糊条件
    /// </summary>
    public string? CreatedByLike { get; set; }


    /// <summary>
    /// 创建时间开始日期
    /// </summary>
    public DateTime? CreatedOnStart { get; set; }

    /// <summary>
    /// 创建时间结束日期
    /// </summary>
    public DateTime? CreatedOnEnd { get; set; }


    /// <summary>
    /// 更新人
    /// </summary>
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// 更新人模糊条件
    /// </summary>
    public string? UpdatedByLike { get; set; }


    /// <summary>
    /// 更新时间开始日期
    /// </summary>
    public DateTime? UpdatedOnStart { get; set; }

    /// <summary>
    /// 更新时间结束日期
    /// </summary>
    public DateTime? UpdatedOnEnd { get; set; }

}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：分页查询对象</para>
/// <para>@描述：生产日历详情;标准分页查询对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-30</para>
/// </summary>
public class PlanCalendarDetailPagedQuery : PagerInfo
{
    /// <summary>
    /// 排序
    /// </summary>
    new public string Sorting { get; set; }

    /// <summary>
    /// 主键
    /// </summary>
    public long? Id { get; set; }

    /// <summary>
    /// 主键组
    /// </summary>
    public IEnumerable<long>? Ids { get; set; }


    /// <summary>
    /// 站点Id
    /// </summary>
    public long? SiteId { get; set; }

    /// <summary>
    /// 站点Id组
    /// </summary>
    public IEnumerable<long>? SiteIds { get; set; }


    /// <summary>
    /// 生产日历Id
    /// </summary>
    public long? PlanCalendarId { get; set; }

    /// <summary>
    /// 生产日历Id组
    /// </summary>
    public IEnumerable<long>? PlanCalendarIds { get; set; }


    /// <summary>
    /// plan_shift id，班制id
    /// </summary>
    public long? ShiftId { get; set; }

    /// <summary>
    /// plan_shift id，班制id组
    /// </summary>
    public IEnumerable<long>? ShiftIds { get; set; }


    /// <summary>
    /// 物料组描述
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 物料组描述模糊条件
    /// </summary>
    public string? RemarkLike { get; set; }


    /// <summary>
    /// 创建人
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// 创建人模糊条件
    /// </summary>
    public string? CreatedByLike { get; set; }


    /// <summary>
    /// 创建时间开始日期
    /// </summary>
    public DateTime? CreatedOnStart { get; set; }

    /// <summary>
    /// 创建时间结束日期
    /// </summary>
    public DateTime? CreatedOnEnd { get; set; }


    /// <summary>
    /// 更新人
    /// </summary>
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// 更新人模糊条件
    /// </summary>
    public string? UpdatedByLike { get; set; }


    /// <summary>
    /// 更新时间开始日期
    /// </summary>
    public DateTime? UpdatedOnStart { get; set; }

    /// <summary>
    /// 更新时间结束日期
    /// </summary>
    public DateTime? UpdatedOnEnd { get; set; }

}