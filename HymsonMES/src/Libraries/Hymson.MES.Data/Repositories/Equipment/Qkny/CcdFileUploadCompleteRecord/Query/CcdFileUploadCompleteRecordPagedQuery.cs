using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.CcdFileUploadCompleteRecord.Query
{
    /// <summary>
    /// CCD文件上传完成 分页参数
    /// </summary>
    public class CcdFileUploadCompleteRecordPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
