using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Report;

namespace Hymson.MES.Services.Services.Report.ManuProductParameterReport
{
    public interface IManuProductParameterReportService
    {
        Task<PagedInfo<ManuProductParameterReportViewDto>> GetManuProductParameterReportPageListAsync(ManuProductParameterReportPagedQueryDto pageQuery);

        Task<ExportResultDto> ManuProductParameterReportExportAsync(ManuProductParameterReportPagedQueryDto pageQuery);
    }
}
