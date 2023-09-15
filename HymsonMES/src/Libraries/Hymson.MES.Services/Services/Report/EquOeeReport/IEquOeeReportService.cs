using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Report;

namespace Hymson.MES.Services.Services.Report.EquHeartbeatReport
{
    /// <summary>
    /// OEE
    /// </summary>
    public interface IEquOeeReportService
    {
        /// <summary>
        /// 查询OEE表分页数据
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquOeeReportViewDto>> GetEquOeeReportPageListAsync(EquOeeReportPagedQueryDto pageQuery);

        /// <summary>
        /// 导出OEE
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        Task<ExportResultDto> EquOeeReportExportAsync(EquOeeReportPagedQueryDto pageQuery);
    }
}
