using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 编码规则物料 分页参数
    /// </summary>
    public class InteCodeRulesMaterialPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
