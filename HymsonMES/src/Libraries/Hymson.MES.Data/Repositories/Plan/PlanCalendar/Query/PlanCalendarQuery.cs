
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Plan;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：查询对象</para>
/// <para>@描述：生产日历;标准查询对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-31</para>
/// </summary>
public class PlanCalendarQuery : QueryAbstraction
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
    /// plan_shift id，班制id
    /// </summary>
    public long? ShiftId { get; set; }

    /// <summary>
    /// plan_shift id，班制id组
    /// </summary>
    public IEnumerable<long>? ShiftIds { get; set; }


    /// <summary>
    /// 年
    /// </summary>
    public int? Year { get; set; }

    /// <summary>
    /// 年组
    /// </summary>
    public IEnumerable<int>? Years { get; set; }


    /// <summary>
    /// 月
    /// </summary>
    public int? Month { get; set; }

    /// <summary>
    /// 月组
    /// </summary>
    public IEnumerable<int>? Months { get; set; }


    /// <summary>
    /// 工作日;工作日，逗号分隔
    /// </summary>
    public string? Workday { get; set; }

    /// <summary>
    /// 工作日;工作日，逗号分隔模糊条件
    /// </summary>
    public string? WorkdayLike { get; set; }


    /// <summary>
    /// 状态;0、未启用 1、启用
    /// </summary>
    public YesOrNoEnum? Status { get; set; }


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


    /// <summary>
    /// 物料组描述
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 物料组描述模糊条件
    /// </summary>
    public string? RemarkLike { get; set; }

}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：分页查询对象</para>
/// <para>@描述：生产日历;标准分页查询对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-31</para>
/// </summary>
public class PlanCalendarPagedQuery : PagerInfo
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
    /// plan_shift id，班制id
    /// </summary>
    public long? ShiftId { get; set; }

    /// <summary>
    /// plan_shift id，班制id组
    /// </summary>
    public IEnumerable<long>? ShiftIds { get; set; }


    /// <summary>
    /// 年
    /// </summary>
    public int? Year { get; set; }

    /// <summary>
    /// 年组
    /// </summary>
    public IEnumerable<int>? Years { get; set; }


    /// <summary>
    /// 月
    /// </summary>
    public int? Month { get; set; }

    /// <summary>
    /// 月组
    /// </summary>
    public IEnumerable<int>? Months { get; set; }


    /// <summary>
    /// 工作日;工作日，逗号分隔
    /// </summary>
    public string? Workday { get; set; }

    /// <summary>
    /// 工作日;工作日，逗号分隔模糊条件
    /// </summary>
    public string? WorkdayLike { get; set; }


    /// <summary>
    /// 状态;0、未启用 1、启用
    /// </summary>
    public YesOrNoEnum? Status { get; set; }


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


    /// <summary>
    /// 物料组描述
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 物料组描述模糊条件
    /// </summary>
    public string? RemarkLike { get; set; }

}