using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Report
{
    /// <summary>
    /// 产品追溯服务
    /// </summary>
    public interface IProductTraceReportService
    {
        /// <summary>
        /// 获取工单信息
        /// </summary>
        /// <param name="planWorkOrderPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProductTracePlanWorkOrderViewDto>> GetWorkOrderPagedListAsync(ProductTracePlanWorkOrderPagedQueryDto planWorkOrderPagedQueryDto);
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

        /// <summary>
        /// 条码生产工艺
        /// </summary>
        /// <param name="procSfcProcessRoutePagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcSfcProcessRouteViewDto>> GetSfcProcessRoutePagedListAsync(ProcSfcProcessRoutePagedQueryDto procSfcProcessRoutePagedQueryDto);

        /// <summary>
        /// NG判定
        /// </summary>
        /// <param name="updateNGJudgeDto"></param>
        /// <returns></returns>
        Task UpdateNGJudgeAsync(UpdateNGJudgeDto updateNGJudgeDto);

        /// <summary>
        /// 产品追朔表导出
        /// </summary>
        /// <param name="planWorkOrderPagedQueryDto"></param>
        /// <returns></returns>
        Task<ExportResultDto> ProductTracingReportExportAsync(ProductTracePagedQueryDto planWorkOrderPagedQueryDto);
    }
}
