using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.CcdFileUploadCompleteRecord
{
    /// <summary>
    /// CCD文件上传完成新增/更新Dto
    /// </summary>
    public record CcdFileUploadCompleteRecordSaveDto : BaseEntityDto
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
        /// 条码是否合格
        /// </summary>
        public int SfcIsPassed { get; set; }

       /// <summary>
        /// 单个文件路径
        /// </summary>
        public string Uri { get; set; }

       /// <summary>
        /// 单个文件是否合格
        /// </summary>
        public int UriIsPassed { get; set; }

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
    /// CCD文件上传完成Dto
    /// </summary>
    public record CcdFileUploadCompleteRecordDto : BaseEntityDto
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
        /// 条码是否合格
        /// </summary>
        public int SfcIsPassed { get; set; }

       /// <summary>
        /// 单个文件路径
        /// </summary>
        public string Uri { get; set; }

       /// <summary>
        /// 单个文件是否合格
        /// </summary>
        public int UriIsPassed { get; set; }

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
    /// CCD文件上传完成分页Dto
    /// </summary>
    public class CcdFileUploadCompleteRecordPagedQueryDto : PagerInfo { }

}
