using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Report.SfcBindParameter
{
    public interface ISfcBindParameterService
    {
        /// <summary>
        /// 报表查询
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<SfcBindParameterReportPagedDataDto>> GetPagedDataAsync(SfcBindParameterReportPagedQueryDto queryDto);

        /// <summary>
        /// 报表导出
        /// </summary>
        /// <returns></returns>
        Task<ExportResultDto> ReportExportAsync(SfcBindParameterReportPagedQueryDto queryDto);
    }
}
