using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 工序配置打印表 分页参数
    /// </summary>
    public class ProcProcedurePrintReleationPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        public long ProcedureId { get; set; }
    }
}
