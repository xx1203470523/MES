using Hymson.MES.Core.Enums;
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

        /// <summary>
        /// 设备Id
        /// </summary>
        public long? EquipmentId { set; get; }
    }

    /// <summary>
    /// 根据工单下达条码返回值
    /// </summary>
    public class CreateBarcodeByWorkOrderOutputBo
    {
        /// <summary>
        /// 条码ID
        /// </summary>
        public long ManuSFCId { get; set; }

        /// <summary>
        /// 产品序列码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public SfcStatusEnum BarcodeStatus { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工序版本
        /// </summary>
        public string ProcedureVersion { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string? MaterialVersion { get; set; }

        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public long ProcessRouteId { get; set; }

        /// <summary>
        /// 工艺路线编码
        /// </summary>
        public string ProcessRouteCode { get; set; }

        /// <summary>
        /// 工艺路线版本
        /// </summary>
        public string ProcessRouteVersion { get; set; }

        /// <summary>
        /// Bom ID
        /// </summary>
        public long? BomId { get; set; }

        /// <summary>
        /// Bom编码
        /// </summary>
        public string? BomCode { get; set; }

        /// <summary>
        /// Bom版本
        /// </summary>
        public string? BomVersion { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CreateBarcodeByResourceCode : CoreBaseBo
    {
        /// <summary>
        /// 资源Id
        /// </summary>
        public string ResourceCode { set; get; }

    }

}
