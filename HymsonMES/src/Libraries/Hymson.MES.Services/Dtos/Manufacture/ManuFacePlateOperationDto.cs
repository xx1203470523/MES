using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Services.Dtos.Manufacture;

/// <summary>
/// 面板确认入参
/// </summary>
public class ManuFacePlateCommonDto
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
}

/// <summary>
/// 面板确认入参（容器编码、单个Sfc）
/// </summary>
public class ManuFacePlatePackDto : ManuFacePlateCommonDto
{
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
/// 移除装载
/// </summary>
public class ManuFacePlateRemovePackedDto
{
    /// <summary>
    /// 包装容器id
    /// </summary>
    public long? PackContainerId { get; set; }

    /// <summary>
    /// 装载id组
    /// </summary>
    public IEnumerable<long>? PackedIds { get; set; }
}

/// <summary>
/// 移除全部装载
/// </summary>
public class ManuFacePlateRemoveAllPackedDto
{
    /// <summary>
    /// 包装容器
    /// </summary>
    public long PackContainerId { get; set; }
}

/// <summary>
/// 打开容器
/// </summary>
public class ManuFacePlateOpenContainerDto
{
    /// <summary>
    /// 包装容器
    /// </summary>
    public long? PackContainerId { get; set; }
}

/// <summary>
/// 关闭容器
/// </summary>
public class ManuFacePlateCloseContainerDto
{
    /// <summary>
    /// 包装容器
    /// </summary>
    public long? PackContainerId { get; set; }
}

/// <summary>
/// 容器列表信息
/// </summary>
public class ManuFacePlateContainerInfoOutputDto
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
/// 容器面板返回信息
/// </summary>
public record ManuFacePlateContainerOutputDto
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
    /// 容器状态
    /// </summary>
    public ManuContainerBarcodeStatusEnum? PackingContainerStatus { get; set; }

    /// <summary>
    /// 最大数量
    /// </summary>
    public decimal? MaxPackCount { get; set; }

    /// <summary>
    /// 最小数量
    /// </summary>
    public decimal? MinPackCount { get; set; }

    /// <summary>
    /// 当前数量
    /// </summary>
    public decimal? CurrentPackCount { get; set; }

    /// <summary>
    /// 提示信息
    /// </summary>
    public string? TipMessage { get; set; }

    /// <summary>
    /// 容器信息
    /// </summary>
    public List<ManuFacePlateContainerInfoOutputDto> ContainerInfoOutputDtos { get; set; }
}

/// <summary>
/// 容器包装面板移除返回
/// </summary>
public record ManuFacePlateContainerRemoveOutputDto
{
    /// <summary>
    /// 容器包装ID
    /// </summary>
    public IEnumerable<long>? ManuContainerPackIds { get; set; }
}

/// <summary>
/// 容器包装面板关闭容器
/// </summary>
public record ManuFacePlateContainerCloseOutputDto
{
    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Success { get; set; }
}