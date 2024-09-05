using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.NIO
{
    /// <summary>
    /// 数据实体（NIO工单数量记录表）   
    /// nio_order_qty
    /// @author Yxx
    /// @date 2024-09-05 02:53:53
    /// </summary>
    public class NioOrderQtyEntity : BaseEntity
    {
        /// <summary>
        /// 工单
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 工单数量
        /// </summary>
        public decimal? Qty { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        
    }
}
