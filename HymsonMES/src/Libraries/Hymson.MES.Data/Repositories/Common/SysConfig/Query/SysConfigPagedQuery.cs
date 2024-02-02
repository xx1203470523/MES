using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Common.Query
{
    /// <summary>
    /// 系统配置 分页参数
    /// </summary>
    public class SysConfigPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
