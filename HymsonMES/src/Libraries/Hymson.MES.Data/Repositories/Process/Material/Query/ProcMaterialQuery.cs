
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories;

namespace Hymson.WMS.Data.Repositories.Process;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：查询对象</para>
/// <para>@描述：物料维护表;标准查询对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-29</para>
/// </summary>
public class ProcMaterialQuery : QueryAbstraction
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
    /// 所属物料组ID
    /// </summary>
    public long? GroupId { get; set; }

    /// <summary>
    /// 所属物料组ID组
    /// </summary>
    public IEnumerable<long>? GroupIds { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; }

    /// <summary>
    /// 物料编码模糊条件
    /// </summary>
    public string? MaterialCodeLike { get; set; }

    /// <summary>
    /// 物料名称
    /// </summary>
    public string? MaterialName { get; set; }

    /// <summary>
    /// 物料名称模糊条件
    /// </summary>
    public string? MaterialNameLike { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public long? Status { get; set; }

    /// <summary>
    /// 状态组
    /// </summary>
    public IEnumerable<long>? Statuss { get; set; }

    /// <summary>
    /// 来源
    /// </summary>
    public long? Origin { get; set; }

    /// <summary>
    /// 来源组
    /// </summary>
    public IEnumerable<long>? Origins { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    public string? Version { get; set; }

    /// <summary>
    /// 版本模糊条件
    /// </summary>
    public string? VersionLike { get; set; }

    /// <summary>
    /// 是否默认版本
    /// </summary>
    public long? IsDefaultVersion { get; set; }

    /// <summary>
    /// 是否默认版本组
    /// </summary>
    public IEnumerable<long>? IsDefaultVersions { get; set; }

    /// <summary>
    /// 物料描述
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 物料描述模糊条件
    /// </summary>
    public string? RemarkLike { get; set; }

    /// <summary>
    /// 采购类型
    /// </summary>
    public long? BuyType { get; set; }

    /// <summary>
    /// 采购类型组
    /// </summary>
    public IEnumerable<long>? BuyTypes { get; set; }

    /// <summary>
    /// 工艺路线ID
    /// </summary>
    public long? ProcessRouteId { get; set; }

    /// <summary>
    /// 工艺路线ID组
    /// </summary>
    public IEnumerable<long>? ProcessRouteIds { get; set; }

    /// <summary>
    /// BomId
    /// </summary>
    public long? BomId { get; set; }

    /// <summary>
    /// BomId组
    /// </summary>
    public IEnumerable<long>? BomIds { get; set; }

    /// <summary>
    /// 批次
    /// </summary>
    public long? Batch { get; set; }

    /// <summary>
    /// 批次组
    /// </summary>
    public IEnumerable<long>? Batchs { get; set; }

    /// <summary>
    /// 标包数量
    /// </summary>
    public long? PackageNum { get; set; }

    /// <summary>
    /// 标包数量组
    /// </summary>
    public IEnumerable<long>? PackageNums { get; set; }

    /// <summary>
    /// 计量单位(字典定义)
    /// </summary>
    public string? Unit { get; set; }

    /// <summary>
    /// 计量单位(字典定义)模糊条件
    /// </summary>
    public string? UnitLike { get; set; }

    /// <summary>
    /// 内/外序列号
    /// </summary>
    public long? SerialNumber { get; set; }

    /// <summary>
    /// 内/外序列号组
    /// </summary>
    public IEnumerable<long>? SerialNumbers { get; set; }

    /// <summary>
    /// 验证掩码组
    /// </summary>
    public string? ValidationMaskGroup { get; set; }

    /// <summary>
    /// 验证掩码组模糊条件
    /// </summary>
    public string? ValidationMaskGroupLike { get; set; }

    /// <summary>
    /// 基于时间(字典定义)
    /// </summary>
    public long? BaseTime { get; set; }

    /// <summary>
    /// 基于时间(字典定义)组
    /// </summary>
    public IEnumerable<long>? BaseTimes { get; set; }

    /// <summary>
    /// 消耗公差
    /// </summary>
    public long? ConsumptionTolerance { get; set; }

    /// <summary>
    /// 消耗公差组
    /// </summary>
    public IEnumerable<long>? ConsumptionTolerances { get; set; }

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
    /// 修改人
    /// </summary>
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// 修改人模糊条件
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

    /// <summary>
    /// 站点Id
    /// </summary>
    public long? SiteId { get; set; }

    /// <summary>
    /// 站点Id组
    /// </summary>
    public IEnumerable<long>? SiteIds { get; set; }

    /// <summary>
    /// 消耗系数最小值
    /// </summary>
    public decimal? ConsumeRatioMin { get; set; }

    /// <summary>
    /// 消耗系数最大值
    /// </summary>
    public decimal? ConsumeRatioMax { get; set; }

    /// <summary>
    /// 掩码规则ID
    /// </summary>
    public long? MaskCodeId { get; set; }

    /// <summary>
    /// 掩码规则ID组
    /// </summary>
    public IEnumerable<long>? MaskCodeIds { get; set; }

}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：分页查询对象</para>
/// <para>@描述：物料维护表;标准分页查询对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-29</para>
/// </summary>
public class ProcMaterialPagedQuery : PagerInfo
{
    /// <summary>
    /// 所属站点代码
    /// </summary>
    public long? SiteId { get; set; }

    /// <summary>
    /// 物料组ID
    /// </summary>
    public long? GroupId { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    public string? Version { get; set; }

    /// <summary>
    /// 物料名称
    /// </summary>
    public string? MaterialName { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public SysDataStatusEnum? Status { get; set; }

    /// <summary>
    /// 来源
    /// </summary>
    public MaterialOriginEnum? Origin { get; set; }

    /// <summary>
    /// 采购类型
    /// </summary>
    public MaterialBuyTypeEnum[]? BuyTypes { get; set; }
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
    /// 所属物料组ID组
    /// </summary>
    public IEnumerable<long>? GroupIds { get; set; }

    /// <summary>
    /// 物料编码模糊条件
    /// </summary>
    public string? MaterialCodeLike { get; set; }

    /// <summary>
    /// 物料名称模糊条件
    /// </summary>
    public string? MaterialNameLike { get; set; }

    /// <summary>
    /// 状态组
    /// </summary>
    public IEnumerable<long>? Statuss { get; set; }

    /// <summary>
    /// 来源组
    /// </summary>
    public IEnumerable<long>? Origins { get; set; }

    /// <summary>
    /// 版本模糊条件
    /// </summary>
    public string? VersionLike { get; set; }

    /// <summary>
    /// 是否默认版本
    /// </summary>
    public long? IsDefaultVersion { get; set; }

    /// <summary>
    /// 是否默认版本组
    /// </summary>
    public IEnumerable<long>? IsDefaultVersions { get; set; }

    /// <summary>
    /// 物料描述
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 物料描述模糊条件
    /// </summary>
    public string? RemarkLike { get; set; }

    /// <summary>
    /// 采购类型
    /// </summary>
    public long? BuyType { get; set; }

    /// <summary>
    /// 工艺路线ID
    /// </summary>
    public long? ProcessRouteId { get; set; }

    /// <summary>
    /// 工艺路线ID组
    /// </summary>
    public IEnumerable<long>? ProcessRouteIds { get; set; }

    /// <summary>
    /// BomId
    /// </summary>
    public long? BomId { get; set; }

    /// <summary>
    /// BomId组
    /// </summary>
    public IEnumerable<long>? BomIds { get; set; }

    /// <summary>
    /// 批次
    /// </summary>
    public long? Batch { get; set; }

    /// <summary>
    /// 批次组
    /// </summary>
    public IEnumerable<long>? Batchs { get; set; }

    /// <summary>
    /// 标包数量
    /// </summary>
    public long? PackageNum { get; set; }

    /// <summary>
    /// 标包数量组
    /// </summary>
    public IEnumerable<long>? PackageNums { get; set; }

    /// <summary>
    /// 计量单位(字典定义)
    /// </summary>
    public string? Unit { get; set; }

    /// <summary>
    /// 计量单位(字典定义)模糊条件
    /// </summary>
    public string? UnitLike { get; set; }

    /// <summary>
    /// 内/外序列号
    /// </summary>
    public long? SerialNumber { get; set; }

    /// <summary>
    /// 内/外序列号组
    /// </summary>
    public IEnumerable<long>? SerialNumbers { get; set; }

    /// <summary>
    /// 验证掩码组
    /// </summary>
    public string? ValidationMaskGroup { get; set; }

    /// <summary>
    /// 验证掩码组模糊条件
    /// </summary>
    public string? ValidationMaskGroupLike { get; set; }

    /// <summary>
    /// 基于时间(字典定义)
    /// </summary>
    public long? BaseTime { get; set; }

    /// <summary>
    /// 基于时间(字典定义)组
    /// </summary>
    public IEnumerable<long>? BaseTimes { get; set; }

    /// <summary>
    /// 消耗公差
    /// </summary>
    public long? ConsumptionTolerance { get; set; }

    /// <summary>
    /// 消耗公差组
    /// </summary>
    public IEnumerable<long>? ConsumptionTolerances { get; set; }

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
    /// 修改人
    /// </summary>
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// 修改人模糊条件
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

    /// <summary>
    /// 站点Id组
    /// </summary>
    public IEnumerable<long>? SiteIds { get; set; }

    /// <summary>
    /// 消耗系数最小值
    /// </summary>
    public decimal? ConsumeRatioMin { get; set; }

    /// <summary>
    /// 消耗系数最大值
    /// </summary>
    public decimal? ConsumeRatioMax { get; set; }

    /// <summary>
    /// 掩码规则ID
    /// </summary>
    public long? MaskCodeId { get; set; }

    /// <summary>
    /// 掩码规则ID组
    /// </summary>
    public IEnumerable<long>? MaskCodeIds { get; set; }

}
