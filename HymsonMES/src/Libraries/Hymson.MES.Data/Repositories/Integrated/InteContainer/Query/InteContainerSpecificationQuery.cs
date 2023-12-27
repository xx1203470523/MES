
using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Inte;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：查询对象</para>
/// <para>@描述：容器规格尺寸表;标准查询对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-13</para>
/// </summary>
public class InteContainerSpecificationQuery : QueryAbstraction
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
    /// 容器 id  inte_container Id
    /// </summary>
    public long? ContainerId { get; set; }

    /// <summary>
    /// 容器 id  inte_container Id组
    /// </summary>
    public IEnumerable<long>? ContainerIds { get; set; }


    /// <summary>
    /// 高度(mm)最小值
    /// </summary>
    public decimal? HeightMin { get; set; }

    /// <summary>
    /// 高度(mm)最大值
    /// </summary>
    public decimal? HeightMax { get; set; }


    /// <summary>
    /// 长度(mm)最小值
    /// </summary>
    public decimal? LengthMin { get; set; }

    /// <summary>
    /// 长度(mm)最大值
    /// </summary>
    public decimal? LengthMax { get; set; }


    /// <summary>
    /// 宽度(mm)最小值
    /// </summary>
    public decimal? WidthMin { get; set; }

    /// <summary>
    /// 宽度(mm)最大值
    /// </summary>
    public decimal? WidthMax { get; set; }


    /// <summary>
    /// 最大填充重量(KG)最小值
    /// </summary>
    public decimal? MaxFillWeightMin { get; set; }

    /// <summary>
    /// 最大填充重量(KG)最大值
    /// </summary>
    public decimal? MaxFillWeightMax { get; set; }


    /// <summary>
    /// 重量(KG)最小值
    /// </summary>
    public decimal? WeightMin { get; set; }

    /// <summary>
    /// 重量(KG)最大值
    /// </summary>
    public decimal? WeightMax { get; set; }


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

}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：分页查询对象</para>
/// <para>@描述：容器规格尺寸表;标准分页查询对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-13</para>
/// </summary>
public class InteContainerSpecificationPagedQuery : PagerInfo
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
    /// 容器 id  inte_container Id
    /// </summary>
    public long? ContainerId { get; set; }

    /// <summary>
    /// 容器 id  inte_container Id组
    /// </summary>
    public IEnumerable<long>? ContainerIds { get; set; }


    /// <summary>
    /// 高度(mm)最小值
    /// </summary>
    public decimal? HeightMin { get; set; }

    /// <summary>
    /// 高度(mm)最大值
    /// </summary>
    public decimal? HeightMax { get; set; }


    /// <summary>
    /// 长度(mm)最小值
    /// </summary>
    public decimal? LengthMin { get; set; }

    /// <summary>
    /// 长度(mm)最大值
    /// </summary>
    public decimal? LengthMax { get; set; }


    /// <summary>
    /// 宽度(mm)最小值
    /// </summary>
    public decimal? WidthMin { get; set; }

    /// <summary>
    /// 宽度(mm)最大值
    /// </summary>
    public decimal? WidthMax { get; set; }


    /// <summary>
    /// 最大填充重量(KG)最小值
    /// </summary>
    public decimal? MaxFillWeightMin { get; set; }

    /// <summary>
    /// 最大填充重量(KG)最大值
    /// </summary>
    public decimal? MaxFillWeightMax { get; set; }


    /// <summary>
    /// 重量(KG)最小值
    /// </summary>
    public decimal? WeightMin { get; set; }

    /// <summary>
    /// 重量(KG)最大值
    /// </summary>
    public decimal? WeightMax { get; set; }


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

}