
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Plan;

/// <summary>
/// <para>@描述：生产日历;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-31</para>
/// <para><seealso cref="BaseEntity">点击查看享元对象</seealso></para>
/// </summary>
public class PlanCalendarEntity : BaseEntity
{

    /// <summary>
    /// 站点Id
    /// </summary>
    public long SiteId { get; set; }


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

}