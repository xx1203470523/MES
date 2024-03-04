using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.WhShipmentMaterial
{
    /// <summary>
    /// 数据实体（出货单物料详情）   
    /// wh_shipment_material
    /// @author Jam
    /// @date 2024-03-04 04:31:26
    /// </summary>
    public class WhShipmentMaterialEntity : BaseEntity
    {
        /// <summary>
        /// 出货单Id
        /// </summary>
        public long ShipmentId { get; set; }

       /// <summary>
        /// 物料Id
        /// </summary>
        public long MaterialId { get; set; }

       /// <summary>
        /// 出货数量
        /// </summary>
        public decimal Qty { get; set; }

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
