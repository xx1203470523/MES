using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Manufacture.Query
{
    /// <summary>
    /// 工装条码绑定 分页参数
    /// </summary>
    public class ManuToolingBindPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
