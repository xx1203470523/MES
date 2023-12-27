using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Core.Domain.Manufacture;

public class ManuContainerPackEntity : BaseEntity
{
    /// <summary>
    /// 站点Id
    /// </summary>
    public long SiteId { get; set; }

    /// <summary>
    /// 资源ID
    /// </summary>
    public long ResourceId { get; set; }
    /// <summary>
    /// 工序ID
    /// </summary>
    public long ProcedureId { get; set; }

    ///// <summary>
    ///// 最外层条码id
    ///// </summary>
    //public long? OutermostContainerBarCodeId { get; set; }

    /// <summary>
    /// 容器条码id
    /// </summary>
    public long? ContainerBarCodeId { get; set; }

    /// <summary>
    /// 装载类型
    /// </summary>
    public ManuContainerBarcodePackTypeEnum? PackType { get; set; }

    /// <summary>
    /// 装载条码
    /// </summary>
    public string LadeBarCode { get; set; }
    
    ///// <summary>
    ///// 装载深度
    ///// </summary>
    //public int? Deep { get; set; }
}
