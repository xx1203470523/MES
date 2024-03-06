namespace Hymson.MES.Data.Repositories.Common.Command;

/// <summary>
/// 批量删除指令
/// </summary>
public class DeleteMoreCommand: UpdateCommandAbstraction
{   
    /// <summary>
    /// Ids
    /// </summary>
    public IEnumerable<long> Ids { get; set; }
}
