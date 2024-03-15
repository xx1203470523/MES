using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.EquProductParamRecord.Query
{
    /// <summary>
    /// 产品参数记录表 分页参数
    /// </summary>
    public class EquProductParamRecordPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
