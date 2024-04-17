using Hymson.MES.CoreServices.Dtos.Manufacture;

namespace Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuCreateBarcodeDto
{
    /// <summary>
    /// 更具工单下达条码
    /// </summary>
    public class CreateBarcodeByWorkOrderDto
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        public long WorkOrderId { set; get; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { set; get; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public long ResourceId { set; get; }
    }    

    /// <summary>
    /// 
    /// </summary>
    public class CreateBarcodeByWorkOrderAndPrintDto
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        public long WorkOrderId { set; get; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { set; get; }

        /// <summary>
        /// 打印机id
        /// </summary>
        public long? PrintId { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public long ResourceId { set; get; }

    }

    /// <summary>
    /// 根据资源下达条码
    /// </summary>
    public class CreateBarcodeByResourceDto
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public long EquipmentId { set; get; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public long ResourceId { set; get; }

        /// <summary>
        /// 数量
        /// </summary>
        public int? Count { set; get; }
    }

    /// <summary>
    /// 根据老条码生成新条码
    /// </summary>
    public class CreateBarcodeByOldMesSfcDto
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public long? EquipmentId { set; get; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public long? ResourceId { set; get; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public long WorkOrderId { set; get; }

        /// <summary>
        /// 就条码
        /// </summary>
        public IEnumerable<BarcodeDto> OldSFCs { set; get; }
    }

    /// <summary>
    /// 根据外部条码接收
    /// </summary>
    public class CreateBarcodeByExternalSfcDto
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public long? EquipmentId { set; get; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public long? ResourceId { set; get; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public long WorkOrderId { set; get; }

        /// <summary>
        /// 就条码
        /// </summary>
        public IEnumerable<BarcodeDto> ExternalSFCs { set; get; }
    }
}
