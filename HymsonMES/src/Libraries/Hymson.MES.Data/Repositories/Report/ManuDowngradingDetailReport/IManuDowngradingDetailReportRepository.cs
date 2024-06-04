using Hymson.Infrastructure;
using Hymson.MES.Data.Repositories.Report.Query;

namespace Hymson.MES.Data.Repositories.Report
{
    /// <summary>
    /// 仓储接口（降级品明细报表）
    /// </summary>
    public interface IManuDowngradingDetailReportRepository
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuDowngradingDetailReportView>> GetPagedInfoAsync(ManuDowngradingDetailReportPagedQuery pagedQuery);

    }
}
