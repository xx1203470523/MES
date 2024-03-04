using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Report;
using Hymson.MES.Services.Dtos.SystemAp;
using Hymson.MES.Services.Dtos.SystemApi;
using Hymson.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.SystemApi;

public interface ISystemApiService
{
    /// <summary>
    /// 首页-工单基本信息
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    Task<IEnumerable<PlanWorkOrderInfoViewDto>> GetPlanWorkOrderInfoAsync(PlanWorkOrderInfoQueryDto queryDto);

    /// <summary>
    /// 首页-OEE趋势图
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    Task<IEnumerable<OEETrendChartViewDto>> GetOEETrendChartAsync(OEETrendChartQueryDto queryDto);

    /// <summary>
    /// 今日一次合格率（风冷/液冷）
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    Task<OneQualifiedViewDto> GetOneQualifiedRateAsync(OneQualifiedQueryDto queryDto);

    /// <summary>
    /// 按月获取一次合格率（风冷/液冷）
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    Task<IEnumerable<OneQualifiedMonthViewDto>> GetMonthOneQualifiedRateAsync(OneQualifiedMonthQueryDto queryDto);

    /// <summary>
    /// 电芯，模组，Pack获取不良分布（日/月，风冷/液冷）
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    Task<IEnumerable<DefectDistributionViewDto>> GetDefectDistributionAsync(DefectDistributionQueryDto queryDto);

    /// <summary>
    /// 工序日产出滚动图
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    Task<IEnumerable<ProcedureDayOutputViewDto>> GetProcedureDayOutputAsync(ProcedureDayOutputQueryDto queryDto);

    /// <summary>
    /// 工序生产产能
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    Task<IEnumerable<ProductionCapacityViewDto>> GetProductionCapacityAsync(ProductionCapacityQueryDto queryDto);

}
