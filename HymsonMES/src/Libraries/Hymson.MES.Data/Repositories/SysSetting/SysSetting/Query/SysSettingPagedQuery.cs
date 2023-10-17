using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.SysSetting.Query
{
    /// <summary>
    /// 配置设置 分页参数
    /// </summary>
    public class SysSettingPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
