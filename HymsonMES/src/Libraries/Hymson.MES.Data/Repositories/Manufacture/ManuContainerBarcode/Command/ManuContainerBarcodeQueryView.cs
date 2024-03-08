using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Data.Repositories.Manufacture;

/// <summary>
/// 数量刷新指令
/// </summary>
public class RefreshStatusCommand
{
    /// <summary>
    /// 操作ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 最大数量
    /// </summary>
    public decimal MaxValue { get;set; }
}

/// <summary>
/// Qty增量指令
/// </summary>
public class IncrementQtyCommand
{
    /// <summary>
    /// 操作ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 增量值
    /// </summary>
    public decimal IncrementValue { get; set; }

    /// <summary>
    /// 最大数量
    /// </summary>
    public decimal MaxValue { get; set; }
}

/// <summary>
/// 清理数量指令
/// </summary>
public class ClearQtyCommand
{
    /// <summary>
    /// 操作ID
    /// </summary>
    public long Id { get; set; }
}

/// <summary>
/// 数量刷新指令
/// </summary>
public class RefreshQtyCommand
{
    /// <summary>
    /// 操作ID
    /// </summary>
    public long Id { get; set; }
}

/// <summary>
/// 关闭容器指令
/// </summary>
public class CloseContainerCommand
{
    /// <summary>
    /// 关闭指令
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 包装容器状态
    /// </summary>
    public ManuContainerBarcodeStatusEnum Status { get; set; } = ManuContainerBarcodeStatusEnum.Close;

    /// <summary>
    /// 允许操作的包装容器状态条件
    /// </summary>
    public ManuContainerBarcodeStatusEnum StatusCondition { get; set; } = ManuContainerBarcodeStatusEnum.Open;
}