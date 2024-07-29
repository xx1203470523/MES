using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Equipment.Query
{
    /// <summary>
    /// 设备点检任务项目 分页参数
    /// </summary>
    public class EquSpotcheckTaskItemPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
