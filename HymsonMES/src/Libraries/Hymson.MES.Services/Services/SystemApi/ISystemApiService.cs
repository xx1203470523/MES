
using Hymson.MES.Services.Dtos.SystemApi;
using Hymson.MES.Services.Dtos.SystemApi.Kanban;

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

    /// <summary>
    /// 设备运行状态
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<EquipmentStatusViewDto>> GetEquipmentStatusAsync();

    /// <summary>
    /// 设备运行状态分布
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<EquStatusDistributionViewDto>> GetEquipmentStatusDistributionAsync();

    /// <summary>
    /// 设备故障率（日/月）
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    Task<IEnumerable<EquFaultRateViewDto>> GetEquFaultRateAsync(EquFaultRateQueryDto queryDto);

    /// <summary>
    /// 设备OEE趋势图（日/月）
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    Task<IEnumerable<EquOEETrendChartViewDto>> GetEquOEETrendChartAsync(EquOEETrendChartQueryDto queryDto);

}
