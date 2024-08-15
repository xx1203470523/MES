using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 设备文件上传新增/更新Dto
    /// </summary>
    public record EquFileUploadSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 工位编码
        /// </summary>
        public string PostionCode { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }

       /// <summary>
        /// Location
        /// </summary>
        public string Location { get; set; }

       /// <summary>
        /// 采集完成时间
        /// </summary>
        public bool CollectionTime { get; set; }

       /// <summary>
        /// 文件扩展名
        /// </summary>
        public string FileExt { get; set; }

       /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize { get; set; }

       /// <summary>
        /// 文件地址
        /// </summary>
        public string FileUrl { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       
    }

    /// <summary>
    /// 设备文件上传Dto
    /// </summary>
    public record EquFileUploadDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

       /// <summary>
        /// 工位编码
        /// </summary>
        public string PostionCode { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }

       /// <summary>
        /// Location
        /// </summary>
        public string Location { get; set; }

       /// <summary>
        /// 采集完成时间
        /// </summary>
        public bool CollectionTime { get; set; }

       /// <summary>
        /// 文件扩展名
        /// </summary>
        public string FileExt { get; set; }

       /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize { get; set; }

       /// <summary>
        /// 文件地址
        /// </summary>
        public string FileUrl { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       
    }

    /// <summary>
    /// 设备文件上传分页Dto
    /// </summary>
    public class EquFileUploadPagedQueryDto : PagerInfo { }

}
