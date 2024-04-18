namespace Hymson.MES.Data.Repositories.Common.Command;

/// <summary>
/// 多条码命令
/// </summary>
public class MultiBarCodeCommand
{
    /// <summary>
    /// 站点Id
    /// </summary>
    public long SiteId { get; set; }

    /// <summary>
    /// 条码集合
    /// </summary>
    public IEnumerable<string> BarCodes { get; set; }

}
