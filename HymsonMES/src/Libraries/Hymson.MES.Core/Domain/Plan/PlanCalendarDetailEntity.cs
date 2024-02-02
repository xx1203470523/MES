
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Plan;

/// <summary>
/// <para>@描述：生产日历详情;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-15</para>
/// <para><seealso cref="BaseEntity">点击查看享元对象</seealso></para>
/// </summary>
public class PlanCalendarDetailEntity : BaseEntity
{

    /// <summary>
    /// 站点Id
    /// </summary>
    public long SiteId { get; set; }

    /// <summary>
    /// 生产日历Id
    /// </summary>
    public long PlanCalendarId { get; set; }

    /// <summary>
    /// 生产日
    /// </summary>
    public int Day { get; set; }


    /// <summary>
    /// plan_shift id，班制id
    /// </summary>
    public long? ShiftId { get; set; }


    /// <summary>
    /// 物料组描述
    /// </summary>
    public string? Remark { get; set; }

}