using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.ManuEuqipmentNewestInfo.Query
{
    /// <summary>
    /// 设备最新信息 分页参数
    /// </summary>
    public class ManuEuqipmentNewestInfoPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
