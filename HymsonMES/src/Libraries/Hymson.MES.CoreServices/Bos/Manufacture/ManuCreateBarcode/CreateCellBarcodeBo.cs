using Hymson.MES.CoreServices.Bos.Common;
using Microsoft.Extensions.Logging;

namespace Hymson.MES.CoreServices.Bos.Manufacture.ManuCreateBarcode
{
    /// <summary>
    /// 创建电芯码
    /// </summary>
    public class CreateCellBarcodeBo : CoreBaseBo
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 极组条码集合
        /// </summary>
        public IEnumerable<string> Barcodes { get; set; }

        /// <summary>
        /// 电芯码是否自动进站
        /// </summary>
        public bool IsInStock { get; set; } = true;
    }

    /// <summary>
    /// 接收电芯码
    /// </summary>
    public class ReceiveCellBarcodeBo : CoreBaseBo
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 线体ID
        /// </summary>
        public long LineId { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// BOM ID
        /// </summary>
        public long BomId { get; set; }

        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public long ProcessRouteId { get; set; }

        /// <summary>
        /// 电芯码
        /// </summary>
        public List<string> SfcList { get; set; } = new List<string>();

        /// <summary>
        /// 电芯码是否自动进站
        /// </summary>
        public bool IsInStock { get; set; } = true;
    }
}
