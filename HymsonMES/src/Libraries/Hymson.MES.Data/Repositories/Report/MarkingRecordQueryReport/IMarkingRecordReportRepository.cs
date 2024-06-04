using Hymson.Infrastructure;
using Hymson.MES.Data.Repositories.Marking.Query;

namespace Hymson.MES.Data.Repositories.Marking
{
    /// <summary>
    /// 仓储接口（Marking拦截汇总表）
    /// </summary>
    public interface IMarkingRecordReportRepository
    {  
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<MarkingRecordQueryReportView>> GetPagedInfoAsync(MarkingReportReportPagedQuery pagedQuery);

    }
}
