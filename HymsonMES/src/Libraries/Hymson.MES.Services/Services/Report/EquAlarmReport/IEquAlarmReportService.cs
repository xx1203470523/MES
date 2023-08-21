using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Report;

namespace Hymson.MES.Services.Services.Report.EquAlarmReport
{
    public interface  IEquAlarmReportService
    {
        Task<PagedInfo<EquAlarmReportViewDto>> GetEquAlarmReportPageListAsync(EquAlarmReportPagedQueryDto pageQuery);

        Task<ExportResultDto> EquAlarmReportExportAsync(EquAlarmReportPagedQueryDto pageQuery);
    }
}
