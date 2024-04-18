using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.ManuFillingDataRecord.Query
{
    /// <summary>
    /// 补液数据上传记录 分页参数
    /// </summary>
    public class ManuFillingDataRecordPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
