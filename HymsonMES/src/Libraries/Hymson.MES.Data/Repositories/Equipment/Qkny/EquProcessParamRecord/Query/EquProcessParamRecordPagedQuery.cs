using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.EquProcessParamRecord.Query
{
    /// <summary>
    /// 过程参数记录表 分页参数
    /// </summary>
    public class EquProcessParamRecordPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
