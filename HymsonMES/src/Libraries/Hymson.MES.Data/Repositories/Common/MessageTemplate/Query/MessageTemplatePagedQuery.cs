using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Common.Query
{
    /// <summary>
    /// 消息模板 分页参数
    /// </summary>
    public class MessageTemplatePagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
