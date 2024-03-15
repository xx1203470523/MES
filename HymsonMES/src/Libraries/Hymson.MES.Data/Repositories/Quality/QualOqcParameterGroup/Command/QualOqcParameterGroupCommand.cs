
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Quality;


public class QualOqcParameterGroupCreateCommand : CreateCommandAbstraction
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
    public long? CustomerId { get; set; }

    /// <summary>
    /// 状态 0、已禁用 2、启用
    /// </summary>
    public DisableOrEnableEnum? Status { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    public string? Version { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 站点ID
    /// </summary>
    public long? SiteId { get; set; }

}


public class QualOqcParameterGroupUpdateCommand : UpdateCommandAbstraction
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
    public long? CustomerId { get; set; }

    /// <summary>
    /// 状态 0、已禁用 2、启用
    /// </summary>
    public DisableOrEnableEnum? Status { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    public string? Version { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 站点ID
    /// </summary>
    public long? SiteId { get; set; }

}
