using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Process.Query
{
    /// <summary>
    /// 工序配置子步骤表 分页参数
    /// </summary>
    public class ProcProcedureSubstepRelationPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

    }
}
