
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Quality;


public class QualFqcParameterGroupCreateCommand : CreateCommandAbstraction
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
    /// 抽检数量
    /// </summary>
    public int SamplingCount { get; set; }

    /// <summary>
    /// 状态(0-新建 1-启用 2-保留 3-废除)
    /// </summary>
    public SysDataStatusEnum? Status { get; set; }

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


public class QualFqcParameterGroupUpdateCommand : UpdateCommandAbstraction
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
    /// 编码
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// 编码模糊条件
    /// </summary>
    public string? CodeLike { get; set; }

    /// <summary>
    /// 抽检数量
    /// </summary>
    public int SamplingCount { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 名称模糊条件
    /// </summary>
    public string? NameLike { get; set; }


    /// <summary>
    /// proc_material id  物料Id
    /// </summary>
    public long? MaterialId { get; set; }

    /// <summary>
    /// proc_material id  物料Id组
    /// </summary>
    public IEnumerable<long>? MaterialIds { get; set; }


    /// <summary>
    /// 客户Id
    /// </summary>
    public long? CustomerId { get; set; }

    /// <summary>
    ///客户id组
    /// </summary>
    public IEnumerable<long>? CustomerIds { get; set; }


    /// <summary>
    /// 状态(0-新建 1-启用 2-保留 3-废除)
    /// </summary>
    public SysDataStatusEnum? Status { get; set; }


    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 备注模糊条件
    /// </summary>
    public string? RemarkLike { get; set; }


    /// <summary>
    /// 创建时间开始日期
    /// </summary>
    public DateTime? CreatedOnStart { get; set; }

    /// <summary>
    /// 创建时间结束日期
    /// </summary>
    public DateTime? CreatedOnEnd { get; set; }


    /// <summary>
    /// 创建人
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// 创建人模糊条件
    /// </summary>
    public string? CreatedByLike { get; set; }


    /// <summary>
    /// 更新时间开始日期
    /// </summary>
    public DateTime? UpdatedOnStart { get; set; }

    /// <summary>
    /// 更新时间结束日期
    /// </summary>
    public DateTime? UpdatedOnEnd { get; set; }


    /// <summary>
    /// 更新人
    /// </summary>
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// 更新人模糊条件
    /// </summary>
    public string? UpdatedByLike { get; set; }


    /// <summary>
    /// 站点ID
    /// </summary>
    public long? SiteId { get; set; }

    /// <summary>
    /// 站点ID组
    /// </summary>
    public IEnumerable<long>? SiteIds { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    public string? Version { get; set; }

}
