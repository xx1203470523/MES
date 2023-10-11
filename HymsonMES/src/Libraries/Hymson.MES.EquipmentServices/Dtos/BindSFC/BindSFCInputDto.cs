using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.EquipmentServices.Dtos.BindSFC;

/// <summary>
/// 条码输入
/// </summary>
public record BindSFCInputDto: BaseDto
{
    /// <summary>
    /// 条码
    /// </summary>
    public string SFC { get; set; } = string.Empty;

    /// <summary>
    /// 操作类型
    /// </summary>
    public RepairOperateTypeEnum OperateType { get; set; }
}

/// <summary>
/// 条码输入
/// </summary>
public record UnBindSFCInputDto : BindSFCInputDto
{
    
    public string[] BindSFCs { get; set; } = Array.Empty<string>();
}

/// <summary>
/// 解绑PDA-无需验证设备
/// </summary>
public record UnBindSFCInput 
{
    /// <summary>
    /// 模组码
    /// </summary>
    public string SFC { get; set; } = string.Empty;
    /// <summary>
    /// 电芯码
    /// </summary>
    public string[] BindSFCs { get; set; } = Array.Empty<string>();
}

/// <summary>
/// 换绑输入
/// </summary>
public record SwitchBindInputDto: BindSFCInputDto
{
    /// <summary>
    /// 旧设备ID
    /// </summary>
    public long OldBindId { get; set; }

    /// <summary>
    /// 旧绑定的电芯SFC
    /// </summary>
    public string OldBindSFC { get; set; } = string.Empty;

    /// <summary>
    /// 新绑定的电芯SFC
    /// </summary>
    public string NewBindSFC { get; set; } = string.Empty;
}

/// <summary>
/// 复投输入
/// </summary>
public record ResumptionInputDto: BindSFCInputDto
{
    /// <summary>
    /// NG位置
    /// </summary>
    public long? NGLocationId { get; set; }

    /// <summary>
    /// 复投位置
    /// </summary>
    public long RepeatLocationId { get; set; }
}

/// <summary>
/// 明细输出
/// </summary>
public record BindSFCOutputDto
{
    /// <summary>
    /// 数据集
    /// </summary>
    public IEnumerable<ManuSfcCirculationSummaryEntity>? Data { get; set; }

    /// <summary>
    /// NG位置
    /// </summary>
    public string? NGState { get; set; }
}
