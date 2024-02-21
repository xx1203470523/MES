
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Qual;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：创建指令</para>
/// <para>@描述：IQC检验项目;标准创建对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// </summary>
public class QualIqcInspectionItemCreateCommand : CreateCommandAbstraction
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
    public YesOrNoEnum? Status { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 站点ID
    /// </summary>
    public long? SiteId { get; set; }

}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@类型：更新指令</para>
/// <para>@描述：IQC检验项目;标准更新对象</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-5</para>
/// </summary>
public class QualIqcInspectionItemUpdateCommand : UpdateCommandAbstraction
{

    /// <summary>
    /// 主键
    /// </summary>
    public long? Id { get; set; }

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
    public YesOrNoEnum? Status { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 站点ID
    /// </summary>
    public long? SiteId { get; set; }

}