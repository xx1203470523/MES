namespace Hymson.MES.CoreServices.Bos.Job;

/// <summary>
/// ESOP获取
/// </summary>
public class EsopOutRequestBo: JobBaseBo
{
    /// <summary>
    /// 工序ID
    /// </summary>
    public long? ProcedureId { get; set; } 

    ///// <summary>
    ///// 物料Id
    ///// </summary>
    ////public long? MaterialId { get; set; }

    /// <summary>
    /// 资源Id
    /// </summary>
    public long? ResourceId { get; set; }


}

/// <summary>
/// ESOP获取
/// </summary>
public class EsopOutResponseBo 
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
