using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Report;

public interface IProductionDetailsReportService
{
    /// <summary>
    /// 获取分页数据
    /// </summary>
    /// <param name="pageQueryDto"></param>
    /// <returns></returns>
    Task<PagedInfo<ProductionDetailsReportDto>> GetPageInfoAsync(ProductionDetailsReportPageQueryDto pageQueryDto);

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    Task<IEnumerable<ProductionDetailsReportDto>> GetListAsync(ProductionDetailsReportQueryDto queryDto);

    /// <summary>
    /// Excel导出
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    Task<ExportResultDto> ExportExcelAsync(ProductionDetailsReportQueryDto queryDto);

}
