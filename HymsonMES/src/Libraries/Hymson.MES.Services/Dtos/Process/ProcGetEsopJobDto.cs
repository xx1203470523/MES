namespace Hymson.MES.Services.Dtos.Process;

/// <summary>
/// ESOPjob查询参数Dto
/// </summary>
public class ProcEsopGetJobQueryDto
{
    /// <summary>
    /// 工序ID
    /// </summary>
    public long? ProcedureId { get; set; }

    /// <summary>
    /// 资源Id
    /// </summary>
    public long? ResourceId { get; set; }
}

/// <summary>
/// ESOPjob返回参数Dto
/// </summary>
public class ProcEsopGetJobOutputDto
{
    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; }

    /// <summary>
    /// 物料名称
    /// </summary>
    public string? MaterialName { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    public string? Version { get; set; }

    /// <summary>
    /// 上载时间
    /// </summary>
    public DateTime? CreatedOn { get; set; }

    /// <summary>
    /// 文件
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 文件Url
    /// </summary>
    public string Path { get; set; }
}
