using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.CcdFileUploadCompleteRecord
{
    /// <summary>
    /// 数据实体（CCD文件上传完成）   
    /// ccd_file_upload_complete_record
    /// @author Yxx
    /// @date 2024-03-08 10:30:50
    /// </summary>
    public class CcdFileUploadCompleteRecordEntity : BaseEntity
    {
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
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        
    }
}
