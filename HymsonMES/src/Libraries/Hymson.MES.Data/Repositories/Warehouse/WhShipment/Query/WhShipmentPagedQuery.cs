using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.WhShipment.Query
{
    /// <summary>
    /// 出货单 分页参数
    /// </summary>
    public class WhShipmentPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 出货单号
        /// </summary>
        public string? ShipmentNum { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime[]? TimeStamp { get; set; }

        /// <summary>
        /// 创建时间开始日期
        /// </summary>
        public DateTime? PlanShipmentTimeStart { get; set; }

        /// <summary>
        /// 创建时间结束日期
        /// </summary>
        public DateTime? PlanShipmentTimeEnd { get; set; }

        /// <summary>
        /// Ids
        /// </summary>
        public IEnumerable<long>? NotInIds { get; set; }

    }
}
