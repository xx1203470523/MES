using Hymson.Utils;

namespace Hymson.MES.Data.Repositories;

/// <summary>
/// 更新指令抽象
/// </summary>
public abstract class UpdateCommandAbstraction : CommandAbstraction
{
    /// <summary>
    /// 操作人
    /// </summary>
    public string UpdatedBy { get; set; }

    /// <summary>
    /// 操作时间
    /// </summary>
    public DateTime UpdatedOn { get; set; }

    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void Init()
    {
        UpdatedBy = "system";
        UpdatedOn = HymsonClock.Now();
    }
}
