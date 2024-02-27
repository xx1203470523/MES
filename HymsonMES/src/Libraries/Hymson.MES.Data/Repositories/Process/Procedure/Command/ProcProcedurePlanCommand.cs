
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Proc;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：创建指令</para>
/// <para>@描述：工序计划产能;标准创建对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-27</para>
/// </summary>
public class ProcProcedurePlanCreateCommand : BaseCommand
{


    /// <summary>
    /// 站点Id
    /// </summary>
    public long? SiteId { get; set; }

    /// <summary>
    /// 工序Id
    /// </summary>
    public long? ProcedureId { get; set; }

    /// <summary>
    /// 计划产能
    /// </summary>
    public decimal? PlanOutputQty { get; set; }

}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：更新指令</para>
/// <para>@描述：工序计划产能;标准更新对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-27</para>
/// </summary>
public class ProcProcedurePlanUpdateCommand : BaseCommand
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
    /// 工序Id
    /// </summary>
    public string? ProcedureId { get; set; }

    /// <summary>
    /// 计划产能
    /// </summary>
    public decimal? PlanOutputQty { get; set; }

}

public class ProcProcedurePlanDeleteCommand : DeleteCommand
{
    /// <summary>
    /// 工序Id
    /// </summary>
    public IEnumerable<long>? ProcedureIds { get; set; }
}