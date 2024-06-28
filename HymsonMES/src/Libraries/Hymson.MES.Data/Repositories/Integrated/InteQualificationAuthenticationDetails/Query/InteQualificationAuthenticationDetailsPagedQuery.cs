using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Integrated.Query
{
    /// <summary>
    /// 角色、人员资质认证 分页参数
    /// </summary>
    public class InteQualificationAuthenticationDetailsPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
