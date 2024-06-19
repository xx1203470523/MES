using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Manufacture.Query
{
    /// <summary>
    /// 联副产品收货 分页参数
    /// </summary>
    public class ManuJointProductAndByproductsReceiveRecordPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
