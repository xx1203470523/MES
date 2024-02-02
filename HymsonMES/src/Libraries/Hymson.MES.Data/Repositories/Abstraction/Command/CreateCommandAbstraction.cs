using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Data.Repositories;

/// <summary>
/// 创建指令抽象
/// </summary>
public abstract class CreateCommandAbstraction : CommandAbstraction
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public string CreatedBy { get; set; }

    /// <summary>
    /// 创建日期
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// 操作人
    /// </summary>
    public string UpdatedBy { get; set; }

    /// <summary>
    /// 操作时间
    /// </summary>
    public DateTime UpdatedOn { get; set; }

    /// <summary>
    /// 是否删除
    /// </summary>
    public long IsDeleted { get; set; } = 0;

    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void Init()
    {
        Id = IdGenProvider.Instance.CreateId();        
        CreatedOn = HymsonClock.Now();
        UpdatedOn = CreatedOn;
        CreatedBy = "system";
        UpdatedBy = "system";
        IsDeleted = 0;        
    }
}
