using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Report;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Report;

public interface IPackBindOtherReportService
{
    /// <summary>
    /// 分页查询数据
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    Task<PagedInfo<PackBindOtherReportViewDto>> GetPagedInfoAsync(PackBindOtherPageQueryPagedDto query);

    /// <summary>
    /// 导出Excel
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    Task<ExportResultDto> ExportExcelAsync([FromQuery] PackBindOtherQueryDto query);
}
