using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.QualificationRateReport.Query;

namespace Hymson.MES.Data.Repositories.QualificationRateReport
{
    /// <summary>
    /// 仓储接口（合格率报表）
    /// </summary>
    public interface IQualificationRateReportRepository
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<QualificationRateReportEnity>> GetPagedInfoAsync(QualificationRateReportPagedQuery pagedQuery);

        /// <summary>
        /// 获取工序列表的信息
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ProcProcedureEntity>> GetProcdureInfoAsync();
    }
}
