
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Qual;

/// <summary>
/// <para>@描述：IQC检验项目; 基础数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// <para><seealso cref="BaseEntityDto">点击查看享元对象</seealso></para>
/// </summary>
public record QualIqcInspectionItemDto : BaseEntityDto
{
    /// <summary>
    /// 编码
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// proc_material id  物料Id
    /// </summary>
    public long? MaterialId { get; set; }

    /// <summary>
    /// wh_supplier id 供应商id
    /// </summary>
    public long? SupplierId { get; set; }

    /// <summary>
    /// 状态 0、已禁用 2、启用
    /// </summary>
    public DisableOrEnableEnum? Status { get; set; }    

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 参数组
    /// </summary>
    public IEnumerable<QualIqcInspectionItemDetailDto>? QualIqcInspectionItemDetailDtos { get; set; }
}

/// <summary>
/// <para>@描述：IQC检验项目; 用于更新数据的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// <para><seealso cref="QualIqcInspectionItemDto">点击查看享元对象</seealso></para>
/// </summary>
public record QualIqcInspectionItemUpdateDto : QualIqcInspectionItemDto
{
    /// <summary>
    /// Id
    /// </summary>
    public long? Id { get; set; }

    /// <summary>
    /// 参数组
    /// </summary>
    new public IEnumerable<QualIqcInspectionItemDetailUpdateDto>? QualIqcInspectionItemDetailDtos { get; set; }
}

/// <summary>
/// <para>@描述：IQC检验项目; 用于页面展示的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// <para><seealso cref="QualIqcInspectionItemUpdateDto">点击查看享元对象</seealso></para>
/// </summary>
public record QualIqcInspectionItemOutputDto : QualIqcInspectionItemUpdateDto
{
    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; }

    /// <summary>
    /// 物料名称
    /// </summary>
    public string? MaterialName {  get; set; }

    /// <summary>
    /// 物料单位
    /// </summary>
    public string? MaterialUnit {  get; set; }

    /// <summary>
    /// 物料版本
    /// </summary>
    public string? MaterialVersion { get; set; }

    /// <summary>
    /// 供应商编码
    /// </summary>
    public string? SupplierCode {  get; set; }

    /// <summary>
    /// 供应商名称
    /// </summary>
    public string? SupplierName {  get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreatedOn { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdatedOn { get; set; }

    /// <summary>
    /// 更新人
    /// </summary>
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// 参数组
    /// </summary>
    new public IEnumerable<QualIqcInspectionItemDetailOutputDto>? QualIqcInspectionItemDetailDtos { get; set; }
}

/// <summary>
/// <para>@描述：IQC检验项目; 用于删除数据的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// </summary>
public record QualIqcInspectionItemDeleteDto : QualIqcInspectionItemDto
{
    /// <summary>
    /// 要删除的组
    /// </summary>
    public IEnumerable<long> Ids { get; set; }
}

/// <summary>
/// <para>@描述：IQC检验项目; 用于条件查询（分页）的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// <para><seealso cref="PagerInfo">点击查看享元对象</seealso></para>
/// </summary>
public class QualIqcInspectionItemPagedQueryDto : PagerInfo
{
    /// <summary>
    /// 编码
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; }

    /// <summary>
    /// 物料名称
    /// </summary>
    public string? MaterialName {  get; set; }

    /// <summary>
    /// 供应商名称
    /// </summary>
    public string? SupplierName {  get; set; }

    /// <summary>
    /// 状态 0、已禁用 2、启用
    /// </summary>
    public DisableOrEnableEnum? Status { get; set; }
}

/// <summary>
/// <para>@描述：IQC检验项目; 用于条件查询的数据传输对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// <para><seealso cref="PagerInfo">点击查看享元对象</seealso></para>
/// </summary>
public class QualIqcInspectionItemQueryDto : QueryDtoAbstraction
{
    /// <summary>
    /// Id
    /// </summary>
    public long? Id { get; set; }
}