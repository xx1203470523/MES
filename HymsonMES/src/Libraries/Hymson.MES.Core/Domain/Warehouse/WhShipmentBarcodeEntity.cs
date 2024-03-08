using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.WhShipmentBarcode
{
    /// <summary>
    /// 数据实体（出货单条码表）   
    /// wh_shipment_barcode
    /// @author Jam
    /// @date 2024-03-04 04:31:43
    /// </summary>
    public class WhShipmentBarcodeEntity : BaseEntity
    {
        /// <summary>
        /// 出货单详情 Id
        /// </summary>
        public long ShipmentDetailId { get; set; }

       /// <summary>
        /// 出货条码
        /// </summary>
        public string BarCode { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
