using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 物料加载，数据实体对象   
    /// manu_feeding
    /// @author Czhipu
    /// @date 2023-03-25 09:56:47
    /// </summary>
    public class ManuFeedingEntity : BaseEntity
    {
        /// <summary>
        /// 资源Id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 上料点
        /// </summary>
        public long? FeedingPointId { get; set; }

        /// <summary>
        /// 上料产品Id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 上料条码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 初始数量
        /// </summary>
        public decimal InitQty { get; set; }

        /// <summary>
        /// 上料数量/卸料数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }


    }
}
