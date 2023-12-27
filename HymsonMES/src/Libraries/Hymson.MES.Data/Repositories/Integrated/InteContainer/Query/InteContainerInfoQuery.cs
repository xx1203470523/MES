
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Inte;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：查询对象</para>
/// <para>@描述：容器维护;标准查询对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-13</para>
/// </summary>
public class InteContainerInfoQuery : QueryAbstraction
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
    /// 容器编码
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// 容器编码模糊条件
    /// </summary>
    public string? CodeLike { get; set; }


    /// <summary>
    /// 容器名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 容器名称模糊条件
    /// </summary>
    public string? NameLike { get; set; }


    /// <summary>
    /// 容器规格描述
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 容器规格描述模糊条件
    /// </summary>
    public string? RemarkLike { get; set; }


    /// <summary>
    /// 状态;0-新建 1-启用 2-保留3-废弃
    /// </summary>
    public SysDataStatusEnum? Status { get; set; }


    /// <summary>
    /// 站点Id
    /// </summary>
    public long? SiteId { get; set; }

    /// <summary>
    /// 站点Id组
    /// </summary>
    public IEnumerable<long>? SiteIds { get; set; }

}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：分页查询对象</para>
/// <para>@描述：容器维护;标准分页查询对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-13</para>
/// </summary>
public class InteContainerInfoPagedQuery : PagerInfo
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
    /// 容器编码
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// 容器编码模糊条件
    /// </summary>
    public string? CodeLike { get; set; }


    /// <summary>
    /// 容器名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 容器名称模糊条件
    /// </summary>
    public string? NameLike { get; set; }


    /// <summary>
    /// 容器规格描述
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 容器规格描述模糊条件
    /// </summary>
    public string? RemarkLike { get; set; }


    /// <summary>
    /// 状态;0-新建 1-启用 2-保留3-废弃
    /// </summary>
    public SysDataStatusEnum? Status { get; set; }


    /// <summary>
    /// 站点Id
    /// </summary>
    public long? SiteId { get; set; }

    /// <summary>
    /// 站点Id组
    /// </summary>
    public IEnumerable<long>? SiteIds { get; set; }

}