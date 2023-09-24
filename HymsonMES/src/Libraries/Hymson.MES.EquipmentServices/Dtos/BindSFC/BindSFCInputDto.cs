using Hymson.MES.Core.Domain.Manufacture;

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
}

/// <summary>
/// 条码输入
/// </summary>
public record UnBindSFCInputDto : BindSFCInputDto
{
    /// <summary>
    /// 解绑的电芯条码列表【我不知道这个前端怎么传，我的经验告诉我这应该是后台通过SFC获取的需要解绑的电芯条码 by hjh】
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
    /// 旧绑定的SFC
    /// </summary>
    public string OldBindSFC { get; set; } = string.Empty;

    /// <summary>
    /// 新绑定的SFC
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
    public long? RepeatLocationId { get; set; }
}

/// <summary>
/// 明细输出
/// </summary>
public record BindSFCOutputDto
{
    /// <summary>
    /// 数据集
    /// </summary>
    public IEnumerable<ManuSfcBindEntity>? Data { get; set; }

    /// <summary>
    /// NG位置
    /// </summary>
    public long? NGLocationId { get; set; }
}
