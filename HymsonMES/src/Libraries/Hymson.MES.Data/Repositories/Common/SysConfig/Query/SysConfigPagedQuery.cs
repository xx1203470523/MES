using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

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

        /// <summary>
        /// 配置类型
        /// </summary>
        public SysConfigEnum ?Type { get; set; }

        /// <summary>
        /// 配置编码
        /// </summary>
        public string ?Code { get; set; }
    }
}
