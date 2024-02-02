
namespace Hymson.MES.Data.Repositories.Plan;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：创建指令</para>
/// <para>@描述：生产日历详情;标准创建对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-30</para>
/// </summary>
public class PlanCalendarDetailCreateCommand : CreateCommandAbstraction
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
    /// plan_shift id，班制id
    /// </summary>
    public long? ShiftId { get; set; }

    /// <summary>
    /// 物料组描述
    /// </summary>
    public string? Remark { get; set; }


}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：更新指令</para>
/// <para>@描述：生产日历详情;标准更新对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-30</para>
/// </summary>
public class PlanCalendarDetailUpdateCommand : UpdateCommandAbstraction
{

    /// <summary>
    /// 主键
    /// </summary>
    public long? Id { get; set; }

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
    /// plan_shift id，班制id
    /// </summary>
    public long? ShiftId { get; set; }

    /// <summary>
    /// 物料组描述
    /// </summary>
    public string? Remark { get; set; }


}