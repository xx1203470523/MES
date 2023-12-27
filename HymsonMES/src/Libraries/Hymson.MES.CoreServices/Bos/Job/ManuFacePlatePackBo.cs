namespace Hymson.MES.CoreServices.Bos.Job;

/// <summary>
/// 面板包装
/// </summary>
public class ManuFacePlatePackBo : JobBaseBo
{
    /// <summary>
    /// 面板编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 工序ID
    /// </summary>
    public long? ProcedureId { get; set; }

    /// <summary>
    /// 工序编码
    /// </summary>
    public string? ProcedureCode { get; set; }

    /// <summary>
    /// 资源ID
    /// </summary>
    public long? ResourceId { get; set; }

    /// <summary>
    /// 资源编码
    /// </summary>
    public string? ResourceCode { get; set; }

    /// <summary>
    /// 在制条码
    /// </summary>
    public IEnumerable<string>? Sfcs { get; set; }

    /// <summary>
    /// 包装容器Id
    /// </summary>
    public long? PackContainerId { get; set; }

    /// <summary>
    /// 包装容器编码
    /// </summary>
    public string? PackContainerCode { get; set; }
}

/// <summary>
/// 面板包装信息
/// </summary>
public class ManuFacePlatePackInfoResponseBo
{
    /// <summary>
    /// 工单ID
    /// </summary>
    public long? WorkOrderId { get; set; }

    /// <summary>
    /// 工单编号
    /// </summary>
    public string? WorkOrderCode { get; set; }

    /// <summary>
    /// 物料ID
    /// </summary>
    public long? MaterialId { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string? MaterialCode { get; set; }

    /// <summary>
    /// 装载信息ID
    /// </summary>
    public long? PackingId { get; set; }

    /// <summary>
    /// 装载的容器条码ID
    /// </summary>
    public long? PackingContainerId { get; set; }

    /// <summary>
    /// 装载的容器条码编码
    /// </summary>
    public string? PackingContainerCode { get; set; }

    /// <summary>
    /// SFC条码 | 容器条码
    /// </summary>
    public string? SFC { get; set; }
}

/// <summary>
/// 面板包装结果
/// </summary>
public record ManuFacePlatePackResponseBo
{
    /// <summary>
    /// 装载容器Id
    /// </summary>
    public long? PackingContainerId { set; get; }

    /// <summary>
    /// 装载容器Code
    /// </summary>
    public string? PackingContainerCode { get; set; }

    /// <summary>
    /// 容器信息
    /// </summary>
    public List<ManuFacePlatePackInfoResponseBo> ContainerInfos { get; set; }
}
