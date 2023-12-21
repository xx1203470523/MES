

using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Report;

/// <summary>
/// PACK码追溯电芯码
/// </summary>
public class PackTraceSfcViewDto : PagerInfo
{
    /// <summary>
    /// Pack码
    /// </summary>
    public string Pack { get; set; }

    /// <summary>
    /// 模组码
    /// </summary>
    public string Module { get; set; }

    /// <summary>
    /// 电芯码
    /// </summary>
    public string SFC { get; set; }
}