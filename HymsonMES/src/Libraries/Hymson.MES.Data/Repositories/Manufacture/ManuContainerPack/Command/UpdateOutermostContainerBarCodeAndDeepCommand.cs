namespace Hymson.MES.Data.Repositories.Manufacture;

public class UpdateOutermostContainerBarCodeAndDeepCommand : UpdateCommandAbstraction
{
    #region 更新内容

    /// <summary>
    /// 最外层容器条码
    /// </summary>
    public long OutermostContainerBarCodeId { get; set; }

    /// <summary>
    /// 深度
    /// </summary>
    public int Deep {  get; set; }

    #endregion

    #region 条件

    /// <summary>
    /// 更新Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 检查更新时间
    /// </summary>
    public DateTime CheckUpdatedOn { get; set; }

    #endregion
}
