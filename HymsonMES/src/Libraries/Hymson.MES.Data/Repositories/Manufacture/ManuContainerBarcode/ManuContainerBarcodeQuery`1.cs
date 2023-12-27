namespace Hymson.MES.Data.Repositories.Manufacture;

/// <summary>
/// 容器条码表 查询参数
/// </summary>
public partial class ManuContainerBarcodeQuery
{
    /// <summary>
    /// 容器条码ID
    /// </summary>
    public long? Id { get; set; }

    /// <summary>
    /// 容器条码组
    /// </summary>
    public IEnumerable<long>? Ids { get; set; }

    /// <summary>
    /// 产品ID
    /// </summary>
    public long? ProductId {  get; set; }

    /// <summary>
    /// 容器规格id
    /// </summary>
    public long? ContainerId {  get; set; }

    /// <summary>
    /// 工单ID
    /// </summary>
    public long? WorkOrderId { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string MaterialCode { get; set; }

    /// <summary>
    /// 物料版本
    /// </summary>
    public string MaterialVersion { get; set; }
}
