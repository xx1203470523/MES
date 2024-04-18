using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.ManuFeedingCompletedZjyjRecord
{
    /// <summary>
    /// manu_feeding_completed_zjyj_record新增/更新Dto
    /// </summary>
    public record ManuFeedingCompletedZjyjRecordSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

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
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public long IsDeleted { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }

    /// <summary>
    /// manu_feeding_completed_zjyj_recordDto
    /// </summary>
    public record ManuFeedingCompletedZjyjRecordDto : BaseEntityDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

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
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public long IsDeleted { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }

    /// <summary>
    /// manu_feeding_completed_zjyj_record分页Dto
    /// </summary>
    public class ManuFeedingCompletedZjyjRecordPagedQueryDto : PagerInfo { }

}
