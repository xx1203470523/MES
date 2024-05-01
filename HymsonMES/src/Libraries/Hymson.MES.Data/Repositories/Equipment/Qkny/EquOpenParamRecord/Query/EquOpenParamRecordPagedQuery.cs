using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.EquOpenParamRecord.Query
{
    /// <summary>
    /// 开机参数记录表 分页参数
    /// </summary>
    public class EquOpenParamRecordPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
