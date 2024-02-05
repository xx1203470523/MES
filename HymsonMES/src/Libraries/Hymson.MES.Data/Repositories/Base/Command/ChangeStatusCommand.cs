using Hymson.MES.Core.Enums;
using Hymson.Utils;

namespace Hymson.MES.Data.Repositories.Common.Command;

/// <summary>
/// 状态变更
/// </summary>
public record ChangeStatusCommand
{
    /// <summary>
    /// 
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 需要变更为的状态
    /// </summary>
    public SysDataStatusEnum Status { get; set; }

    /// <summary>
    /// 更新人
    /// </summary>
    public string UpdatedBy { get; set; } = "";

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdatedOn { get; set; } = HymsonClock.Now();
}
