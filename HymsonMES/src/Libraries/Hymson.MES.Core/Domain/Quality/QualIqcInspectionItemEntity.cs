
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Qual;

/// <summary>
/// <para>@描述：IQC检验项目;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// <para><seealso cref="BaseEntity">点击查看享元对象</seealso></para>
/// </summary>
public class QualIqcInspectionItemEntity : BaseEntity
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
    /// 状态 0、已禁用 1、已启用
    /// </summary>
    public DisableOrEnableEnum? Status { get; set; }


    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }


    /// <summary>
    /// 站点ID
    /// </summary>
    public long SiteId { get; set; }

}