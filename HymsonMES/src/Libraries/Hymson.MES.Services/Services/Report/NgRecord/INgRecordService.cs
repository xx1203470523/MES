using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Report;

public interface INgRecordService
{
    /// <summary>
    /// 获取分页数据
    /// </summary>
    /// <param name="pageQueryDto"></param>
    /// <returns></returns>
    Task<PagedInfo<NgRecordReportDto>> GetPageInfoAsync(NgRecordReportPageQueryDto pageQueryDto);

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    Task<IEnumerable<NgRecordReportDto>> GetListAsync(NgRecordReportQueryDto queryDto);

    /// <summary>
    /// Excel导出
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    Task<ExportResultDto> ExportExcelAsync(NgRecordReportQueryDto queryDto);

}
