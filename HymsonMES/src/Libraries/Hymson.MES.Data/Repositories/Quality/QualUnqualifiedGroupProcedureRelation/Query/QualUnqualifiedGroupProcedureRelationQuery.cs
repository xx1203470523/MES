
using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Quality;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：查询对象</para>
/// <para>@描述：不合格组关联工序表;标准查询对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-3-12</para>
/// </summary>
public class QualUnqualifiedGroupProcedureRelationQuery : QueryAbstraction
{

    /// <summary>
    /// 
    /// </summary>
    public long? Id { get; set; }

    /// <summary>
    /// 组
    /// </summary>
    public IEnumerable<long>? Ids { get; set; }


    /// <summary>
    /// 所属站点代码
    /// </summary>
    public long? SiteId { get; set; }

    /// <summary>
    /// 所属站点代码组
    /// </summary>
    public IEnumerable<long>? SiteIds { get; set; }


    /// <summary>
    /// 所属不合格组ID
    /// </summary>
    public long? UnqualifiedGroupId { get; set; }

    /// <summary>
    /// 所属不合格组ID组
    /// </summary>
    public IEnumerable<long>? UnqualifiedGroupIds { get; set; }


    /// <summary>
    /// 所属工序ID
    /// </summary>
    public long? ProcedureId { get; set; }

    /// <summary>
    /// 所属工序ID组
    /// </summary>
    public IEnumerable<long>? ProcedureIds { get; set; }


    /// <summary>
    /// 说明
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 说明模糊条件
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
    /// 最后修改人
    /// </summary>
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// 最后修改人模糊条件
    /// </summary>
    public string? UpdatedByLike { get; set; }


    /// <summary>
    /// 修改时间开始日期
    /// </summary>
    public DateTime? UpdatedOnStart { get; set; }

    /// <summary>
    /// 修改时间结束日期
    /// </summary>
    public DateTime? UpdatedOnEnd { get; set; }

}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：分页查询对象</para>
/// <para>@描述：不合格组关联工序表;标准分页查询对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-3-12</para>
/// </summary>
public class QualUnqualifiedGroupProcedureRelationPagedQuery : PagerInfo
{
    /// <summary>
    /// 排序
    /// </summary>
    new public string Sorting { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public long? Id { get; set; }

    /// <summary>
    /// 组
    /// </summary>
    public IEnumerable<long>? Ids { get; set; }


    /// <summary>
    /// 所属站点代码
    /// </summary>
    public long? SiteId { get; set; }

    /// <summary>
    /// 所属站点代码组
    /// </summary>
    public IEnumerable<long>? SiteIds { get; set; }


    /// <summary>
    /// 所属不合格组ID
    /// </summary>
    public long? UnqualifiedGroupId { get; set; }

    /// <summary>
    /// 所属不合格组ID组
    /// </summary>
    public IEnumerable<long>? UnqualifiedGroupIds { get; set; }


    /// <summary>
    /// 所属工序ID
    /// </summary>
    public long? ProcedureId { get; set; }

    /// <summary>
    /// 所属工序ID组
    /// </summary>
    public IEnumerable<long>? ProcedureIds { get; set; }


    /// <summary>
    /// 说明
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 说明模糊条件
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
    /// 最后修改人
    /// </summary>
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// 最后修改人模糊条件
    /// </summary>
    public string? UpdatedByLike { get; set; }


    /// <summary>
    /// 修改时间开始日期
    /// </summary>
    public DateTime? UpdatedOnStart { get; set; }

    /// <summary>
    /// 修改时间结束日期
    /// </summary>
    public DateTime? UpdatedOnEnd { get; set; }

}