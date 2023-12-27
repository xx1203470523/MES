
using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Inte;

/// <summary>
/// <para>@描述：容器规格尺寸表; 基础数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-14</para>
/// <para><seealso cref="BaseEntityDto">点击查看享元对象</seealso></para>
/// </summary>
public record InteContainerSpecificationDto : BaseEntityDto
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

}

/// <summary>
/// <para>@描述：容器规格尺寸表; 用于更新数据的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-14</para>
/// <para><seealso cref="InteContainerSpecificationDto">点击查看享元对象</seealso></para>
/// </summary>
public record InteContainerSpecificationUpdateDto : InteContainerSpecificationDto
{
    /// <summary>
    /// Id
    /// </summary>
    public long? Id { get; set; }
}

/// <summary>
/// <para>@描述：容器规格尺寸表; 用于页面展示的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-14</para>
/// <para><seealso cref="InteContainerSpecificationUpdateDto">点击查看享元对象</seealso></para>
/// </summary>
public record InteContainerSpecificationOutputDto : InteContainerSpecificationUpdateDto
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

}

/// <summary>
/// <para>@描述：容器规格尺寸表; 用于删除数据的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-14</para>
/// </summary>
public record InteContainerSpecificationDeleteDto : InteContainerSpecificationDto
{
    /// <summary>
    /// 要删除的组
    /// </summary>
    public IEnumerable<long> Ids { get; set; }
}

/// <summary>
/// <para>@描述：容器规格尺寸表; 用于条件查询（分页）的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-14</para>
/// <para><seealso cref="PagerInfo">点击查看享元对象</seealso></para>
/// </summary>
public class InteContainerSpecificationPagedQueryDto : PagerInfo
{
}

/// <summary>
/// <para>@描述：容器规格尺寸表; 用于条件查询（分页）的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-14</para>
/// <para><seealso cref="PagerInfo">点击查看享元对象</seealso></para>
/// </summary>
public class InteContainerSpecificationQueryDto : QueryDtoAbstraction
{
}