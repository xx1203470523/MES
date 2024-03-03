
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Data.Repositories.Inte;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：创建指令</para>
/// <para>@描述：容器装载维护;标准创建对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-13</para>
/// </summary>
public class InteContainerFreightCreateCommand : CreateCommandAbstraction
{


    /// <summary>
    /// 容器 id  inte_container_info Id
    /// </summary>
    public long? ContainerId { get; set; }

    /// <summary>
    /// 类型 1、物料 2、物料组 3、容器
    /// </summary>
    public ContainerFreightTypeEnum? Type { get; set; }

    /// <summary>
    /// 物料id
    /// </summary>
    public long? MaterialId { get; set; }

    /// <summary>
    /// 物料组Id
    /// </summary>
    public long? MaterialGroupId { get; set; }

    /// <summary>
    /// 装载容器 id  inte_container Id
    /// </summary>
    public long? FreightContainerId { get; set; }

    /// <summary>
    /// 最小用量
    /// </summary>
    public decimal? Minimum { get; set; }

    /// <summary>
    /// 最大用量
    /// </summary>
    public decimal? Maximum { get; set; }

    /// <summary>
    /// 容器规格描述
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 站点Id
    /// </summary>
    public long? SiteId { get; set; }

    /// <summary>
    /// 包装等级值
    /// </summary>
    public string? LevelValue { get; set; }

}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：更新指令</para>
/// <para>@描述：容器装载维护;标准更新对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-13</para>
/// </summary>
public class InteContainerFreightUpdateCommand : UpdateCommandAbstraction
{

    /// <summary>
    /// 主键
    /// </summary>
    public long? Id { get; set; }

    /// <summary>
    /// 容器 id  inte_container_info Id
    /// </summary>
    public long? ContainerId { get; set; }

    /// <summary>
    /// 类型 1、物料 2、物料组 3、容器
    /// </summary>
    public ContainerFreightTypeEnum? Type { get; set; }

    /// <summary>
    /// 物料id
    /// </summary>
    public long? MaterialId { get; set; }

    /// <summary>
    /// 物料组Id
    /// </summary>
    public long? MaterialGroupId { get; set; }

    /// <summary>
    /// 装载容器 id  inte_container Id
    /// </summary>
    public long? FreightContainerId { get; set; }

    /// <summary>
    /// 最小用量
    /// </summary>
    public decimal? Minimum { get; set; }

    /// <summary>
    /// 最大用量
    /// </summary>
    public decimal? Maximum { get; set; }

    /// <summary>
    /// 容器规格描述
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 站点Id
    /// </summary>
    public long? SiteId { get; set; }

    /// <summary>
    /// 包装等级值
    /// </summary>
    public string? LevelValue { get; set; }

}