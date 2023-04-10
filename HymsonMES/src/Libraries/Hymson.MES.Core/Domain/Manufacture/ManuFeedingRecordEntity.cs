using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 上卸料记录表，数据实体对象   
    /// manu_feeding_record
    /// @author Czhipu
    /// @date 2023-03-27 05:08:17
    /// </summary>
    public class ManuFeedingRecordEntity : BaseEntity
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
        /// 产品Id
        /// </summary>
        public long? ProductId { get; set; }

        /// <summary>
        /// 上料条码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Qty { get; set; }

        /// <summary>
        /// 上/卸料方向;1：上料；2：卸料；
        /// </summary>
        public FeedingDirectionTypeEnum DirectionType { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }


    }
}