using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.ManuFeedingTransferRecord
{
    /// <summary>
    /// 上料信息转移记录新增/更新Dto
    /// </summary>
    public record ManuFeedingTransferRecordSaveDto : BaseEntityDto
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
        /// 条码
        /// </summary>
        public string Sfc { get; set; } = string.Empty;

       /// <summary>
        /// 转出罐体设备号
        /// </summary>
        public string EquipmentCodeOut { get; set; } = string.Empty;

       /// <summary>
        /// 转入罐体设备号
        /// </summary>
        public string EquipmentCodeIn { get; set; } = string.Empty;

       /// <summary>
        /// 重量
        /// </summary>
        public decimal? Qty { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; } = string.Empty;

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; } = string.Empty;

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
        public string Remark { get; set; } = string.Empty;

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }

    /// <summary>
    /// 上料信息转移记录Dto
    /// </summary>
    public record ManuFeedingTransferRecordDto : BaseEntityDto
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
        /// 条码
        /// </summary>
        public string Sfc { get; set; }

       /// <summary>
        /// 转出罐体设备号
        /// </summary>
        public string EquipmentCodeOut { get; set; }

       /// <summary>
        /// 转入罐体设备号
        /// </summary>
        public string EquipmentCodeIn { get; set; }

       /// <summary>
        /// 重量
        /// </summary>
        public decimal? Qty { get; set; }

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
    /// 上料信息转移记录分页Dto
    /// </summary>
    public class ManuFeedingTransferRecordPagedQueryDto : PagerInfo { }

}
