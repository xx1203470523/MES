using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.ManuFeedingNoProductionRecord
{
    /// <summary>
    /// 设备投料非生产投料(洗罐子)新增/更新Dto
    /// </summary>
    public record ManuFeedingNoProductionRecordSaveDto : BaseEntityDto
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
        /// 投料至设备
        /// </summary>
        public string ConsumeEquipmentCode { get; set; }

       /// <summary>
        /// 投料至资源
        /// </summary>
        public string ConsumeResourceCodeCode { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }

       /// <summary>
        /// 重量
        /// </summary>
        public decimal? Qty { get; set; }

       /// <summary>
        /// 分类
        /// </summary>
        public string Category { get; set; }

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
    /// 设备投料非生产投料(洗罐子)Dto
    /// </summary>
    public record ManuFeedingNoProductionRecordDto : BaseEntityDto
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
        /// 投料至设备
        /// </summary>
        public string ConsumeEquipmentCode { get; set; }

       /// <summary>
        /// 投料至资源
        /// </summary>
        public string ConsumeResourceCodeCode { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }

       /// <summary>
        /// 重量
        /// </summary>
        public decimal? Qty { get; set; }

       /// <summary>
        /// 分类
        /// </summary>
        public string Category { get; set; }

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
    /// 设备投料非生产投料(洗罐子)分页Dto
    /// </summary>
    public class ManuFeedingNoProductionRecordPagedQueryDto : PagerInfo { }

}
