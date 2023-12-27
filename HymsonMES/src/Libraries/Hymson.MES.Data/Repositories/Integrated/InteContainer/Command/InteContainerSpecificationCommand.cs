
namespace Hymson.MES.Data.Repositories.Inte;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：创建指令</para>
/// <para>@描述：容器规格尺寸表;标准创建对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-13</para>
/// </summary>
public class InteContainerSpecificationCreateCommand : CreateCommandAbstraction
{


    /// <summary>
    /// 容器 id  inte_container Id
    /// </summary>
    public long? ContainerId { get; set; }

    /// <summary>
    /// 高度(mm)
    /// </summary>
    public decimal? Height { get; set; }

    /// <summary>
    /// 长度(mm)
    /// </summary>
    public decimal? Length { get; set; }

    /// <summary>
    /// 宽度(mm)
    /// </summary>
    public decimal? Width { get; set; }

    /// <summary>
    /// 最大填充重量(KG)
    /// </summary>
    public decimal? MaxFillWeight { get; set; }

    /// <summary>
    /// 重量(KG)
    /// </summary>
    public decimal? Weight { get; set; }

    /// <summary>
    /// 容器规格描述
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 站点Id
    /// </summary>
    public long? SiteId { get; set; }

}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：更新指令</para>
/// <para>@描述：容器规格尺寸表;标准更新对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-13</para>
/// </summary>
public class InteContainerSpecificationUpdateCommand : UpdateCommandAbstraction
{

    /// <summary>
    /// 主键
    /// </summary>
    public long? Id { get; set; }

    /// <summary>
    /// 容器 id  inte_container Id
    /// </summary>
    public long? ContainerId { get; set; }

    /// <summary>
    /// 高度(mm)
    /// </summary>
    public decimal? Height { get; set; }

    /// <summary>
    /// 长度(mm)
    /// </summary>
    public decimal? Length { get; set; }

    /// <summary>
    /// 宽度(mm)
    /// </summary>
    public decimal? Width { get; set; }

    /// <summary>
    /// 最大填充重量(KG)
    /// </summary>
    public decimal? MaxFillWeight { get; set; }

    /// <summary>
    /// 重量(KG)
    /// </summary>
    public decimal? Weight { get; set; }

    /// <summary>
    /// 容器规格描述
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 站点Id
    /// </summary>
    public long? SiteId { get; set; }

}