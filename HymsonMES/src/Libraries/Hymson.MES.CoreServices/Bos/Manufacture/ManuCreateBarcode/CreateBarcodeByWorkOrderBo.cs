using Hymson.MES.CoreServices.Bos.Common;

namespace Hymson.MES.CoreServices.Bos.Manufacture.ManuCreateBarcode
{
    /// <summary>
    /// 根据工单下达条码
    /// </summary>
    public class CreateBarcodeByWorkOrderBo : CoreBaseBo
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
    public class CreateBarcodeBySemiProductId : CoreBaseBo
    {
        /// <summary>
        /// 资源Id
        /// </summary>
        public string ResourceCode { set; get; }

    }

}
