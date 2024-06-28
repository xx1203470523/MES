using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Process.Query
{
    /// <summary>
    /// 资源资质认证（物理删除） 分页参数
    /// </summary>
    public class ProcResourceQualificationAuthenticationRelationPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
