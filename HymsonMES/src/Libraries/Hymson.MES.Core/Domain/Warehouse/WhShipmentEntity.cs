using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.WhShipment
{
    /// <summary>
    /// 数据实体（出货单）   
    /// wh_shipment
    /// @author Jam
    /// @date 2024-03-04 04:16:33
    /// </summary>
    public class WhShipmentEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 出货单号
        /// </summary>
        public string ShipmentNum { get; set; }

       /// <summary>
        /// 客户id
        /// </summary>
        public long CustomerId { get; set; }

       /// <summary>
        /// 计划出现时间
        /// </summary>
        public string PlanShipmentTime { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       
    }
}
