
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Services.Dtos.Inte;

/// <summary>
/// <para>@描述：容器维护; 基础数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-14</para>
/// <para><seealso cref="BaseEntityDto">点击查看享元对象</seealso></para>
/// </summary>
public record InteContainerInfoDto : BaseEntityDto
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
    /// 容器规格尺寸
    /// </summary>
    public InteContainerSpecificationDto? ContainerSpecification { get; set; }

    /// <summary>
    /// 容器装载维护组
    /// </summary>
    public IEnumerable<InteContainerFreightDto>? FreightGroups { get; set; }

}

/// <summary>
/// <para>@描述：容器维护; 用于更新数据的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-14</para>
/// <para><seealso cref="InteContainerInfoDto">点击查看享元对象</seealso></para>
/// </summary>
public record InteContainerInfoUpdateDto : InteContainerInfoDto
{
    /// <summary>
    /// Id
    /// </summary>
    public long? Id { get; set; }

    /// <summary>
    /// 容器规格尺寸
    /// </summary>
    public new InteContainerSpecificationUpdateDto? ContainerSpecification { get; set; }

    /// <summary>
    /// 容器装载维护组
    /// </summary>
    public new IEnumerable<InteContainerFreightUpdateDto>? FreightGroups { get; set; }
}

/// <summary>
/// <para>@描述：容器维护; 用于页面展示的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-14</para>
/// <para><seealso cref="InteContainerInfoUpdateDto">点击查看享元对象</seealso></para>
/// </summary>
public record InteContainerInfoOutputDto : InteContainerInfoUpdateDto
{
    /// <summary>
    /// 创建人;创建人
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// 创建时间;创建时间
    /// </summary>
    public DateTime? CreatedOn { get; set; }

    /// <summary>
    /// 更新人;更新人
    /// </summary>
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// 更新时间;更新时间
    /// </summary>
    public DateTime? UpdatedOn { get; set; }

    /// <summary>
    /// 容器规格尺寸
    /// </summary>
    public new InteContainerSpecificationOutputDto? ContainerSpecification { get; set; }

    /// <summary>
    /// 容器装载维护组
    /// </summary>
    public new IEnumerable<InteContainerFreightOutputDto>? FreightGroups { get; set; }
}

/// <summary>
/// <para>@描述：容器维护; 用于删除数据的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-14</para>
/// </summary>
public record InteContainerInfoDeleteDto : InteContainerInfoDto
{
    /// <summary>
    /// 要删除的组
    /// </summary>
    public IEnumerable<long> Ids { get; set; }
}

/// <summary>
/// <para>@描述：容器维护; 用于条件查询（分页）的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-14</para>
/// <para><seealso cref="PagerInfo">点击查看享元对象</seealso></para>
/// </summary>
public class InteContainerInfoPagedQueryDto : PagerInfo
{
    /// <summary>
    /// 物料名称/物料组名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    public string? Version { get; set; }

    /// <summary>
    /// 定义方式;0-物料，1-物料组
    /// </summary>
    public DefinitionMethodEnum? DefinitionMethod { get; set; }

    /// <summary>
    /// 包装等级（分为一级/二级/三级）
    /// </summary>
    public LevelEnum? Level { get; set; }

    /// <summary>
    /// 状态;0-新建 1-启用 2-保留3-废弃
    /// </summary>
    public SysDataStatusEnum? Status { get; set; }
}

/// <summary>
/// <para>@描述：容器维护; 用于条件查询（分页）的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-14</para>
/// <para><seealso cref="PagerInfo">点击查看享元对象</seealso></para>
/// </summary>
public class InteContainerInfoQueryDto : QueryDtoAbstraction
{
    /// <summary>
    /// 容器名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 容器编码
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// 状态;0-新建 1-启用 2-保留3-废弃
    /// </summary>
    public SysDataStatusEnum? Status { get; set; }
}

public class InteContainerQueryDto
{
    /// <summary>
    /// 容器编码
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// 产线编码
    /// </summary>
    public string? WorkCenterCode { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; }

    /// <summary>
    /// 工单ID
    /// </summary>
    public long? Id { get; set; }

    /// <summary>
    /// 物料Id
    /// </summary>
    public long? ProductId {  get; set; }
}