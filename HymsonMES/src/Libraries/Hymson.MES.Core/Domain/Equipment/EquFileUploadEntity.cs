using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（设备文件上传）   
    /// equ_file_upload
    /// @author User
    /// @date 2024-08-15 10:06:10
    /// </summary>
    public class EquFileUploadEntity : BaseEntity
    {
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
        public DateTime CollectionTime { get; set; }

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

        
    }
}
