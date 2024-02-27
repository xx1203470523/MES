
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Proc;

/// <summary>
/// <para>@描述：工序计划产能;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-27</para>
/// <para><seealso cref="BaseEntity">点击查看享元对象</seealso></para>
/// </summary>
public class ProcProcedurePlanEntity : BaseEntity
{

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