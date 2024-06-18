using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Common.Query
{
    /// <summary>
    /// 系统配置 查询参数
    /// </summary>
    public class SysConfigQuery
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 配置类型
        /// </summary>
        public SysConfigEnum Type { get; set; }

        /// <summary>
        /// 配置编码
        /// </summary>
        public IEnumerable<string> Codes { get; set; }

    }
}
