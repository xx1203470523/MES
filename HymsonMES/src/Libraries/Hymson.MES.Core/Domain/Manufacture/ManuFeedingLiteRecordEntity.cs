using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 上卸料记录表（轻量），数据实体对象   
    /// manu_feeding_lite_record
    /// @author Czhipu
    /// @date 2023-05-19 09:43:55
    /// </summary>
    public class ManuFeedingLiteRecordEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }

       /// <summary>
        /// 资源Id
        /// </summary>
        public long ResourceId { get; set; }

       /// <summary>
        /// 传输时间
        /// </summary>
        public DateTime LocalTime { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }

       /// <summary>
        /// 上/卸料方向;1：上料；2：卸料；
        /// </summary>
        public FeedingDirectionTypeEnum DirectionType { get; set; }

       /// <summary>
        /// 数量;允许为空，空则取原材料条码解析数据
        /// </summary>
        public decimal? Qty { get; set; }

       /// <summary>
        /// 卸料类型;2：卸料剩余物料 3：卸料剩余物料并报废
        /// </summary>
        public FeedingUnloadingTypeEnum? UnloadingType { get; set; }

       /// <summary>
        /// 是否上料点;空默认不是上料点 叠片公用的为true，独立为空或false
        /// </summary>
        public bool? IsFeedingPoint { get; set; }

       
    }
}
