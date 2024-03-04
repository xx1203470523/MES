
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Data.Repositories.Inte;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：查询对象</para>
/// <para>@描述：容器装载维护;标准查询对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-13</para>
/// </summary>
public class InteContainerFreightQuery : QueryAbstraction
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
    /// 容器 id  inte_container_info Id
    /// </summary>
    public long? ContainerId { get; set; }

    /// <summary>
    /// 容器 id  inte_container_info Id组
    /// </summary>
    public IEnumerable<long>? ContainerIds { get; set; }


    /// <summary>
    /// 类型 1、物料 2、物料组 3、容器
    /// </summary>
    public ContainerFreightTypeEnum? Type { get; set; }


    /// <summary>
    /// 物料id
    /// </summary>
    public long? MaterialId { get; set; }

    /// <summary>
    /// 物料id组
    /// </summary>
    public IEnumerable<long>? MaterialIds { get; set; }


    /// <summary>
    /// 物料组Id
    /// </summary>
    public long? MaterialGroupId { get; set; }

    /// <summary>
    /// 物料组Id组
    /// </summary>
    public IEnumerable<long>? MaterialGroupIds { get; set; }


    /// <summary>
    /// 装载容器 id  inte_container Id
    /// </summary>
    public long? FreightContainerId { get; set; }

    /// <summary>
    /// 装载容器 id  inte_container Id组
    /// </summary>
    public IEnumerable<long>? FreightContainerIds { get; set; }


    /// <summary>
    /// 最小用量最小值
    /// </summary>
    public decimal? MinimumMin { get; set; }

    /// <summary>
    /// 最小用量最大值
    /// </summary>
    public decimal? MinimumMax { get; set; }


    /// <summary>
    /// 最大用量最小值
    /// </summary>
    public decimal? MaximumMin { get; set; }

    /// <summary>
    /// 最大用量最大值
    /// </summary>
    public decimal? MaximumMax { get; set; }


    /// <summary>
    /// 容器规格描述
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 容器规格描述模糊条件
    /// </summary>
    public string? RemarkLike { get; set; }


    /// <summary>
    /// 创建人;创建人
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// 创建人;创建人模糊条件
    /// </summary>
    public string? CreatedByLike { get; set; }


    /// <summary>
    /// 创建时间;创建时间开始日期
    /// </summary>
    public DateTime? CreatedOnStart { get; set; }

    /// <summary>
    /// 创建时间;创建时间结束日期
    /// </summary>
    public DateTime? CreatedOnEnd { get; set; }


    /// <summary>
    /// 更新人;更新人
    /// </summary>
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// 更新人;更新人模糊条件
    /// </summary>
    public string? UpdatedByLike { get; set; }


    /// <summary>
    /// 更新时间;更新时间开始日期
    /// </summary>
    public DateTime? UpdatedOnStart { get; set; }

    /// <summary>
    /// 更新时间;更新时间结束日期
    /// </summary>
    public DateTime? UpdatedOnEnd { get; set; }


    /// <summary>
    /// 站点Id
    /// </summary>
    public long? SiteId { get; set; }

    /// <summary>
    /// 站点Id组
    /// </summary>
    public IEnumerable<long>? SiteIds { get; set; }


    /// <summary>
    /// 包装等级值
    /// </summary>
    public string? LevelValue { get; set; }

    /// <summary>
    /// 包装等级值模糊条件
    /// </summary>
    public string? LevelValueLike { get; set; }

}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：分页查询对象</para>
/// <para>@描述：容器装载维护;标准分页查询对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-13</para>
/// </summary>
public class InteContainerFreightPagedQuery : PagerInfo
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
    /// 容器 id  inte_container_info Id
    /// </summary>
    public long? ContainerId { get; set; }

    /// <summary>
    /// 容器 id  inte_container_info Id组
    /// </summary>
    public IEnumerable<long>? ContainerIds { get; set; }


    /// <summary>
    /// 类型 1、物料 2、物料组 3、容器
    /// </summary>
    public ContainerFreightTypeEnum? Type { get; set; }


    /// <summary>
    /// 物料id
    /// </summary>
    public long? MaterialId { get; set; }

    /// <summary>
    /// 物料id组
    /// </summary>
    public IEnumerable<long>? MaterialIds { get; set; }


    /// <summary>
    /// 物料组Id
    /// </summary>
    public long? MaterialGroupId { get; set; }

    /// <summary>
    /// 物料组Id组
    /// </summary>
    public IEnumerable<long>? MaterialGroupIds { get; set; }


    /// <summary>
    /// 装载容器 id  inte_container Id
    /// </summary>
    public long? FreightContainerId { get; set; }

    /// <summary>
    /// 装载容器 id  inte_container Id组
    /// </summary>
    public IEnumerable<long>? FreightContainerIds { get; set; }


    /// <summary>
    /// 最小用量最小值
    /// </summary>
    public decimal? MinimumMin { get; set; }

    /// <summary>
    /// 最小用量最大值
    /// </summary>
    public decimal? MinimumMax { get; set; }


    /// <summary>
    /// 最大用量最小值
    /// </summary>
    public decimal? MaximumMin { get; set; }

    /// <summary>
    /// 最大用量最大值
    /// </summary>
    public decimal? MaximumMax { get; set; }


    /// <summary>
    /// 容器规格描述
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 容器规格描述模糊条件
    /// </summary>
    public string? RemarkLike { get; set; }


    /// <summary>
    /// 创建人;创建人
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// 创建人;创建人模糊条件
    /// </summary>
    public string? CreatedByLike { get; set; }


    /// <summary>
    /// 创建时间;创建时间开始日期
    /// </summary>
    public DateTime? CreatedOnStart { get; set; }

    /// <summary>
    /// 创建时间;创建时间结束日期
    /// </summary>
    public DateTime? CreatedOnEnd { get; set; }


    /// <summary>
    /// 更新人;更新人
    /// </summary>
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// 更新人;更新人模糊条件
    /// </summary>
    public string? UpdatedByLike { get; set; }


    /// <summary>
    /// 更新时间;更新时间开始日期
    /// </summary>
    public DateTime? UpdatedOnStart { get; set; }

    /// <summary>
    /// 更新时间;更新时间结束日期
    /// </summary>
    public DateTime? UpdatedOnEnd { get; set; }


    /// <summary>
    /// 站点Id
    /// </summary>
    public long? SiteId { get; set; }

    /// <summary>
    /// 站点Id组
    /// </summary>
    public IEnumerable<long>? SiteIds { get; set; }


    /// <summary>
    /// 包装等级值
    /// </summary>
    public string? LevelValue { get; set; }

    /// <summary>
    /// 包装等级值模糊条件
    /// </summary>
    public string? LevelValueLike { get; set; }

}