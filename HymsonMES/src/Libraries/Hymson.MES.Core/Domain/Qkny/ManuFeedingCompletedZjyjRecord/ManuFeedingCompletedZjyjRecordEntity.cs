using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.ManuFeedingCompletedZjyjRecord
{
    /// <summary>
    /// 数据实体（manu_feeding_completed_zjyj_record）   
    /// manu_feeding_completed_zjyj_record
    /// @author Yxx
    /// @date 2024-03-15 11:04:42
    /// </summary>
    public class ManuFeedingCompletedZjyjRecordEntity : BaseEntity
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 上料前重量
        /// </summary>
        public decimal BeforeFeedingQty { get; set; }

        /// <summary>
        /// 上料后重量
        /// </summary>
        public decimal? AfterFeedingQty { get; set; }

        /// <summary>
        /// 上料重量
        /// </summary>
        public decimal? FeedingQty { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        
    }
}
