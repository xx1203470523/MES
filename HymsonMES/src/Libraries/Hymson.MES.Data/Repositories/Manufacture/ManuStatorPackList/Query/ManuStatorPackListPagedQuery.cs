using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Manufacture.Query
{
    /// <summary>
    /// 定子装箱记录表 分页参数
    /// </summary>
    public class ManuStatorPackListPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 箱体码
        /// </summary>
        public string? BoxCode { get; set; }

        /// <summary>
        /// 成品码
        /// </summary>
        public string? ProductCode { get; set; }
    }
}
