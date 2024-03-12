
namespace Hymson.MES.Data.Repositories.Quality;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：创建指令</para>
/// <para>@描述：不合格组关联工序表;标准创建对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-3-12</para>
/// </summary>
public class QualUnqualifiedGroupProcedureRelationCreateCommand
{
    /// <summary>
    /// 所属站点代码
    /// </summary>
    public long? SiteId { get; set; }

    /// <summary>
    /// 所属不合格组ID
    /// </summary>
    public long? UnqualifiedGroupId { get; set; }

    /// <summary>
    /// 所属工序ID
    /// </summary>
    public long? ProcedureId { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：更新指令</para>
/// <para>@描述：不合格组关联工序表;标准更新对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-3-12</para>
/// </summary>
public class QualUnqualifiedGroupProcedureRelationUpdateCommand 
{

    /// <summary>
    /// 
    /// </summary>
    public long? Id { get; set; }

    /// <summary>
    /// 所属站点代码
    /// </summary>
    public long? SiteId { get; set; }

    /// <summary>
    /// 所属不合格组ID
    /// </summary>
    public long? UnqualifiedGroupId { get; set; }

    /// <summary>
    /// 所属工序ID
    /// </summary>
    public long? ProcedureId { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    public string? Remark { get; set; }

}