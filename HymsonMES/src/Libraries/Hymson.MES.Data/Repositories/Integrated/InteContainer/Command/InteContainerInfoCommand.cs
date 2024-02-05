
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Inte;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：创建指令</para>
/// <para>@描述：容器维护;标准创建对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-13</para>
/// </summary>
public class InteContainerInfoCreateCommand : CreateCommandAbstraction
{


    /// <summary>
    /// 容器编码
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// 容器名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 容器规格描述
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 状态;0-新建 1-启用 2-保留3-废弃
    /// </summary>
    public SysDataStatusEnum? Status { get; set; }

    /// <summary>
    /// 站点Id
    /// </summary>
    public long? SiteId { get; set; }

}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：更新指令</para>
/// <para>@描述：容器维护;标准更新对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-13</para>
/// </summary>
public class InteContainerInfoUpdateCommand : UpdateCommandAbstraction
{

    /// <summary>
    /// 主键
    /// </summary>
    public long? Id { get; set; }

    /// <summary>
    /// 容器编码
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// 容器名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 容器规格描述
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 状态;0-新建 1-启用 2-保留3-废弃
    /// </summary>
    public SysDataStatusEnum? Status { get; set; }

    /// <summary>
    /// 站点Id
    /// </summary>
    public long? SiteId { get; set; }

}