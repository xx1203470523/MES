using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hymson.MES.SystemServices.Dtos.ProductTraceReport;
using Hymson.MES.SystemServices.Dtos.ProductTraceReport.query;

namespace Hymson.MES.SystemServices.Services.ProductTrace;

/// <summary>
/// 产品追溯服务
/// </summary>
public interface IProductTraceReportService
{
    /// <summary>
    /// 获取条码追溯信息
    /// </summary>
    /// <param name="productTraceReportPagedQueryDto"></param>
    /// <returns></returns>
    Task<PagedInfo<ManuSfcCirculationViewDto>> GetProductTracePagedListAsync(ProductTracePagedQueryDto productTraceReportPagedQueryDto);
    /// <summary>
    /// 获取参数信息
    /// </summary>
    /// <param name="manuProductPrameterPagedQueryDto"></param>
    /// <returns></returns>
    Task<PagedInfo<ManuProductParameterViewDto>> GetProductPrameterPagedListAsync(ManuProductPrameterPagedQueryDto manuProductPrameterPagedQueryDto);
    /// <summary>
    /// 条码履历
    /// </summary>
    /// <param name="manuSfcStepPagedQueryDto"></param>
    /// <returns></returns>
    Task<PagedInfo<ManuSfcStepViewDto>> GetSfcStepPagedListAsync(ManuSfcStepPagedQueryDto manuSfcStepPagedQueryDto);
}
