using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Process.Query
{
    /// <summary>
    /// 子步骤 分页参数
    /// </summary>
    public class ProcProcedureSubstepPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
